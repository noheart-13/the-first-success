using NUnit.Framework.Internal;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GmaeManager : MonoBehaviour
{
    private int score=0;
    public  GameObject gameOverPanel;
    public GameObject meunPanel;
    public TextMeshProUGUI scoreText;
    private SpawnManager3 spawnManager;
    private void Start()
    {
        UpdateScoreUI();
        spawnManager = GetComponent<SpawnManager3>();
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreUI();
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        spawnManager.StpoSpawn();
    }
    public void StartGame(int difficulty)
    {
        meunPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        spawnManager.StartSpawn(difficulty);
    }
    void UpdateScoreUI()
    {
        scoreText.text = "·ÖÊý£º " + score;
    }
    public void RestartButtonOnClick()
    {
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
