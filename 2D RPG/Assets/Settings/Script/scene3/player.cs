using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rig;
    private AudioSource backgroundMusicSource;
    private WaveMananger waveManager;
    private Vector2 startPosition;
    private float startRotation;

    public float gravityScale = 1f;
    public float jumpForce = 100f;
    public bool isGameOver = false;

    private bool isGrounded = true;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = gravityScale;

        backgroundMusicSource = GetComponent<AudioSource>();
        waveManager = FindAnyObjectByType<WaveMananger>();
        startPosition = rig.position;
        startRotation = rig.rotation;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0f);
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            EndGame();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void EndGame()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0f;
        backgroundMusicSource?.Stop();
        waveManager?.StopSpawning();
        Debug.Log("Game over. Press R to restart.");
    }

    private void RestartGame()
    {
        foreach (Moveleft movingObject in FindObjectsByType<Moveleft>())
        {
            movingObject.ResetForRestart();
        }

        rig.position = startPosition;
        rig.rotation = startRotation;
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0f;

        isGrounded = false;
        isGameOver = false;

        if (waveManager == null)
        {
            waveManager = FindAnyObjectByType<WaveMananger>();
        }

        waveManager?.RestartSpawning();

        if (backgroundMusicSource != null && backgroundMusicSource.clip != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.Play();
        }
    }
}
