using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private SwitchScript switchScript;
    [SerializeField] private Animator anim;

    public string nextLevel;

    private bool isDoorOpen = false;
    
    public bool isDoorActive()
    {
        return isDoorOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if(switchScript.isSwtichOn() && !isDoorOpen)
        {  
            isDoorOpen = true;
            anim.SetBool("SwitchOn",true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isDoorOpen)
        {
            LevelStatus nextLevelStatus = LevelManager.levelInstance.GetLevelStatus(nextLevel);
            if(nextLevelStatus == LevelStatus.Locked)
                LevelManager.levelInstance.SetLevelStatus(nextLevel,LevelStatus.Unlocked);
            string currentLevel = SceneManager.GetActiveScene().name;
            LevelManager.levelInstance.SetLevelStatus(currentLevel,LevelStatus.Completed);
            SceneManager.LoadScene(nextLevel);
        }
    }
}
