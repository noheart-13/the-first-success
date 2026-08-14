using UnityEngine;

public class Moveleft : MonoBehaviour
{
    public float speed = 10f;

    [SerializeField] private float destroyX = -15f;
    [SerializeField] private float destroyY = -6f;

    private player playerScript;
    private bool isRepeatingBackground;
    private Vector3 startPosition;

    void Start()
    {
        playerScript = FindAnyObjectByType<player>();
        isRepeatingBackground = TryGetComponent<RepeatBackground>(out _);
        startPosition = transform.position;
    }

    void Update()
    {
        if (playerScript == null || playerScript.isGameOver)
        {
            return;
        }

        transform.Translate(Vector3.left * (speed * Time.deltaTime));

        if (!isRepeatingBackground && (transform.position.x < destroyX || transform.position.y < destroyY))
        {
            Destroy(gameObject);
        }
    }

    public void ResetForRestart()
    {
        if (isRepeatingBackground)
        {
            transform.position = startPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
