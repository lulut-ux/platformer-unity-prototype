using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverUI;
    int score = 0;

    void Start()
    {
        if (gameOverUI) gameOverUI.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (gameOverUI) gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
