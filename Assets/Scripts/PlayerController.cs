using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.75f;
    public float runSpeed = 3f;
    //public float speed = 5f;
    public Text scoreText;
    public GameObject explosionPrefab;

    public UnityEngine.Rendering.Universal.Light2D flashlightBeam;
    public UnityEngine.Rendering.Universal.Light2D playerAura;

    public AudioClip collectSound;
    public AudioClip footstepSound;

    private AudioSource audioSource;
    private int score = 0;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator animator;
    private SpriteRenderer spriteRn;

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

        UpdateScoreText();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlightBeam != null)
                flashlightBeam.enabled = !flashlightBeam.enabled;

            //if (playerAura != null)
                //playerAura.enabled = !playerAura.enabled;
        }

        // --- ËÎÃ²ÊÀ Á²ÃÓ (SHIFT) ---
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            animator.speed = 1.5f;
            audioSource.pitch = 1.5f;
        }
        else
        {
            currentSpeed = walkSpeed;
            animator.speed = 1f;
            audioSource.pitch = 1f;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        if (moveInput.sqrMagnitude > 0)
        {
            animator.SetBool("IsMoving", true);
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            animator.SetBool("IsMoving", false);
            if (audioSource.isPlaying) audioSource.Stop();
        }

        if (moveInput.x > 0)
        {
            spriteRn.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            spriteRn.flipX = true;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Diamant"))
        {
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
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