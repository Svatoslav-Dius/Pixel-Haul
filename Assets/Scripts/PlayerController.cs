using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.75f;  // Повільна хода (для атмосфери)
    public float runSpeed = 3f;   // Біг (для втечі)
    //public float speed = 5f;       // Швидкість руху
    public Text scoreText;         // Посилання на текст лічильника
    public GameObject explosionPrefab;

    public UnityEngine.Rendering.Universal.Light2D flashlightBeam; // Сюди тягнемо об'єкт "Flashlight"
    public UnityEngine.Rendering.Universal.Light2D playerAura;

    public AudioClip collectSound;
    public AudioClip footstepSound;

    private AudioSource audioSource;
    private int score = 0;         // Змінна для рахунку
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator animator;       // Посилання на аніматор
    private SpriteRenderer spriteRn; // Посилання на картинку (щоб дзеркалити)

    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRn = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (footstepSound != null)
        {
            audioSource.clip = footstepSound;
            audioSource.loop = true;
        }

        UpdateScoreText(); // Показати 0 на старті
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlightBeam != null)
                flashlightBeam.enabled = !flashlightBeam.enabled; // Перемикаємо промінь

            if (playerAura != null)
                playerAura.enabled = !playerAura.enabled;         // Перемикаємо ауру
        }

        // --- ЛОГІКА БІГУ (SHIFT) ---
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed; // Біжимо
            animator.speed = 1.5f;   // Пришвидшуємо анімацію ніг (для краси)
            audioSource.pitch = 1.5f;
        }
        else
        {
            currentSpeed = walkSpeed; // Йдемо
            animator.speed = 1f;      // Нормальна анімація
            audioSource.pitch = 1f;
        }

        // Отримуємо введення з клавіатури (WASD або стрілки)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        if (moveInput.sqrMagnitude > 0)
        {
            animator.SetBool("IsMoving", true); // Вмикаємо біг
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            animator.SetBool("IsMoving", false); // Вмикаємо стійку
            if (audioSource.isPlaying) audioSource.Stop();
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
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
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

            //if (GameObject.FindGameObjectsWithTag("Diamant").Length <= 1)
            //{
            //    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            //    SceneManager.LoadScene(nextSceneIndex);
            //}
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