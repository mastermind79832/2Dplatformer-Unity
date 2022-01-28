using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public KeyUiManager keyUI;
    public HealthManager HealthUI;
    public GameObject gameOverPanel;

    void Start() 
    {
        gameOverPanel.SetActive(false);
    }
    
    public void KeysObtained(int index)
    {
        keyUI.keyCollected(index);
    }

    public void HealthLost()
    {
        HealthUI.SetHealthLost();
    }
    
    public int GetRemainingHealth()
    {
        return HealthUI.GetHealthCount();
    }

    public void ResetHealth()
    {
        HealthUI.ResetHealth();
    }

    public void SetGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
