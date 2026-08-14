using UnityEngine;

public class shot : MonoBehaviour
{
    public float speed = 5f; // 每秒移动的单位数
    private int direction = 1; // 1=右，-1=左

    public void SetDirection(int newDirection)
    {
        direction = newDirection;
    }

    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * speed * direction; // 根据方向移动
    }
}
