using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector2 startPosition;
    private float xBound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        xBound = startPosition.x - 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < xBound)
        {
            transform.position = startPosition;
        }
    }
}
