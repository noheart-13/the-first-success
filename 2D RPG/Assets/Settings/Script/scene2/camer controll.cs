using UnityEngine;

public class camercontroll : MonoBehaviour
{
    public GameObject player;

    private readonly Vector3 offset = new(0f, 2f, -10f);

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
