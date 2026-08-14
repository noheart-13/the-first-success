using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 4f;
    public float bulletOffset = 1f;
    public float jumpForce = 6f;
    public float gravityScale = 2f;

    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;

    private readonly HashSet<Collider2D> groundContacts = new();
    private readonly HashSet<Collider2D> obstacleContacts = new();
    private Rigidbody2D body;
    private AudioSource backgroundMusicSource;
    private float horizontalInput;
    private int facingDirection = 1;
    private bool jumpQueued;
    private bool musicPausedByObstacle;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody2D>();
        }

        body.bodyType = RigidbodyType2D.Dynamic;
        body.gravityScale = gravityScale;
        body.constraints |= RigidbodyConstraints2D.FreezeRotation;

        backgroundMusicSource = GetComponent<AudioSource>();
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        }

        if (backgroundMusicClip != null)
        {
            backgroundMusicSource.clip = backgroundMusicClip;
        }

        backgroundMusicSource.loop = true;
        backgroundMusicSource.playOnAwake = false;
        backgroundMusicSource.volume = musicVolume;
    }

    private void Start()
    {
        if (backgroundMusicSource.clip != null)
        {
            backgroundMusicSource.Play();
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0f)
        {
            facingDirection = horizontalInput > 0f ? 1 : -1;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            && groundContacts.Count > 0)
        {
            jumpQueued = true;
            groundContacts.Clear();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (jumpQueued)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpQueued = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundContact(collision);

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            RegisterObstacle(collision.collider);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts.Remove(collision.collider);

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            UnregisterObstacle(collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            RegisterObstacle(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            UnregisterObstacle(other);
        }
    }

    private void UpdateGroundContact(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.6f)
            {
                groundContacts.Add(collision.collider);
                return;
            }
        }

        groundContacts.Remove(collision.collider);
    }

    private void RegisterObstacle(Collider2D obstacle)
    {
        if (obstacleContacts.Add(obstacle) && obstacleContacts.Count == 1)
        {
            backgroundMusicSource.Pause();
            musicPausedByObstacle = true;
        }
    }

    private void UnregisterObstacle(Collider2D obstacle)
    {
        obstacleContacts.Remove(obstacle);

        if (obstacleContacts.Count == 0 && musicPausedByObstacle)
        {
            backgroundMusicSource.UnPause();
            musicPausedByObstacle = false;
        }
    }

    private void Fire()
    {
        if (bullet == null)
        {
            Debug.LogError("Bullet prefab is not assigned.", this);
            return;
        }

        Vector3 spawnPosition = transform.position + Vector3.right * (bulletOffset * facingDirection);
        GameObject bulletInstance = Instantiate(bullet, spawnPosition, Quaternion.identity);

        shot bulletScript = bulletInstance.GetComponent<shot>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(facingDirection);
        }
    }
}
