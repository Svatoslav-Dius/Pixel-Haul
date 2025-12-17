using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;       // Швидкість руху
    public Text scoreText;         // Посилання на текст лічильника
    public GameObject explosionPrefab;

    public AudioClip collectSound;
    private AudioSource audioSource;

    private int score = 0;         // Змінна для рахунку
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator animator;       // Посилання на аніматор
    private SpriteRenderer spriteRn; // Посилання на картинку (щоб дзеркалити)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRn = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText(); // Показати 0 на старті
    }

    void Update()
    {
        // Отримуємо введення з клавіатури (WASD або стрілки)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        if (moveInput.sqrMagnitude > 0)
        {
            //transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
            animator.SetBool("IsMoving", true); // Вмикаємо біг
        }
        else
        {
            //transform.localScale = new Vector3(0.225f, 0.225f, 0.225f);
            animator.SetBool("IsMoving", false); // Вмикаємо стійку
        }

        // 2. Дзеркалення (Поворот вліво/вправо)
        if (moveInput.x > 0)
        {
            spriteRn.flipX = false; // Дивиться вправо (оригінал)
        }
        else if (moveInput.x < 0)
        {
            spriteRn.flipX = true;  // Дивиться вліво (віддзеркалений)
        }
    }

    void FixedUpdate()
    {
        // Рухаємо персонажа
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    // Взаємодія з предметами (блоками)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Diamant")) // Перевіряємо, чи це блок
        {
            // СТВОРЮЄМО ВИБУХ
            // Instantiate(що, де, який поворот)
            Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);

            audioSource.PlayOneShot(collectSound);

            score++;
            Destroy(other.gameObject);
            UpdateScoreText();

            if (GameObject.FindGameObjectsWithTag("Diamant").Length <= 1)
            {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                SceneManager.LoadScene(nextSceneIndex);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            // Перезавантажуємо поточну сцену (рівень спочатку)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Той самий код перезапуску
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}