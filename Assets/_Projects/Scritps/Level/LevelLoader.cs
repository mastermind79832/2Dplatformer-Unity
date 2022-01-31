using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        levelButtons = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform level = transform.GetChild(i);
            levelButtons[i] = level.gameObject;

            TextMeshProUGUI levelText = level.GetChild(0).GetComponent<TextMeshProUGUI>();
            string levelName = levelText.text;

            Button levelButton = level.GetComponent<Button>();
            LevelStatus status = LevelManager.levelInstance.GetLevelStatus(levelName);
            if (status == LevelStatus.Locked)
            {
                levelButton.interactable = false;
                levelText.color = Color.red;
            }
            else if (status == LevelStatus.Unlocked)
            {
                levelButton.interactable = true;
                levelText.color = Color.white;
            }
            else
            {
                levelButton.interactable = true;
                levelText.color = Color.green;
            }
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ResetAllProgress()
    {
        LevelManager.levelInstance.ResetProgress();

        Initialization();
    }
}
