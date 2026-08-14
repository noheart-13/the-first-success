using UnityEngine;

public class StartButton : MonoBehaviour
{
    private GmaeManager gameManager;
    public int difficulty = 1;
    private void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GmaeManager>();
    }
    public void StartGame()
    {
        gameManager.StartGame(difficulty);
    }

}
