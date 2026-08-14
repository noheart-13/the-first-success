using UnityEngine;

public class Autorotation : MonoBehaviour
{
    public float rotationSpeed = 360f; // 每秒旋转360度
    
    void Update()
    {
        // 2D旋转：绕Z轴旋转
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
