using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager s_LevelInstance;
    public static LevelManager levelInstance { get { return s_LevelInstance;}}

    void Awake()
    {
        if(s_LevelInstance == null)
        {
            s_LevelInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if(GetLevelStatus("Level 1") == LevelStatus.Locked)
            SetLevelStatus("Level 1",LevelStatus.Unlocked);
    }

    public LevelStatus GetLevelStatus(string level)
    {
        LevelStatus status = (LevelStatus)PlayerPrefs.GetInt(level, 0);
        return status;
    }
    public void SetLevelStatus( string level, LevelStatus status )
    {
        PlayerPrefs.SetInt(level, (int)status);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
