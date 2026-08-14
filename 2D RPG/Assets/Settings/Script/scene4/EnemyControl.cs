using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;

    public float Moveforce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (rb == null)
        {
            Debug.LogError("EnemyControl requires a Rigidbody2D.", this);
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            enabled = false;
            return;
        }

        Vector2 direction =
            (player.transform.position - transform.position).normalized;

        rb.AddForce(direction * Moveforce);
    }
}
