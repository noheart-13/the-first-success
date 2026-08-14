using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody2D rgb;

    public float upMinforce = 12f;
    public float upMaxforce = 15f;
    public float torqueRange = 2f;

    public int scoreValue = 5;
    private GmaeManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        rgb.AddForce(RandomForce(), ForceMode2D.Impulse);
        rgb.AddTorque(RandomTorque().y, ForceMode2D.Impulse);
        gameManager=GameObject.Find("Manager").GetComponent<GmaeManager>();
    }
    Vector2 RandomForce()
    {
        return Vector2.up * Random.Range(upMinforce, upMaxforce);
    }
    Vector2 RandomTorque()
    {
        return Vector2.up * Random.Range(-torqueRange, torqueRange);
    }
    private void OnMouseDown()
    {
        Destroy(gameObject);
        gameManager.UpdateScore(scoreValue);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("Good"))
        {
            gameManager.GameOver();
        }
            Destroy(gameObject);
    }
}
