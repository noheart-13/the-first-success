using System.Collections;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    private Rigidbody2D rgb;
    private SpriteRenderer spriteRenderer;
    private GameObject foculpoint;
    private Coroutine powerupCoroutine;

    public float speed = 5f;
    public bool hasPowerup = false;
    public float powerupStrength = 10f;

    [SerializeField] private float powerupDuration = 5f;
    [SerializeField] private Color normalColor = Color.red;
    [SerializeField] private Color powerupColor = Color.blue;

    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        foculpoint = GameObject.Find("foculpoint");

        if (rgb == null || foculpoint == null)
        {
            Debug.LogError("Playercontroller requires Rigidbody2D and foculpoint.", this);
            enabled = false;
            return;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        rgb.AddForce(foculpoint.transform.up * verticalInput * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Powerup"))
        {
            return;
        }

        Destroy(collision.gameObject);
        hasPowerup = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = powerupColor;
        }

        if (powerupCoroutine != null)
        {
            StopCoroutine(powerupCoroutine);
        }

        powerupCoroutine = StartCoroutine(PowerupCountdown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasPowerup || !collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Rigidbody2D enemyRgb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (enemyRgb == null)
        {
            return;
        }

        Vector2 awayFromPlayer =
            (collision.transform.position - transform.position).normalized;

        enemyRgb.AddForce(
            awayFromPlayer * powerupStrength,
            ForceMode2D.Impulse);
    }

    private IEnumerator PowerupCountdown()
    {
        yield return new WaitForSeconds(powerupDuration);

        hasPowerup = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }

        powerupCoroutine = null;
    }
}
