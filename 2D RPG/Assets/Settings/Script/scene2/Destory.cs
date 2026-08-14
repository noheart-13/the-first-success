using UnityEngine;

public class Destory : MonoBehaviour
{
    public float destroyDistance = 9f; // 销毁距离（9米）
    private GameObject player;

    void Start()
    {
        // 查找挂载 Move 脚本的物体
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            // 如果没有标签，尝试查找 Move 组件
            Move moveScript = FindAnyObjectByType<Move>();
            if (moveScript != null)
            {
                player = moveScript.gameObject;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // 计算与 Move 物体的距离
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
