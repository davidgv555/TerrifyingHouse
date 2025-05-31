using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static void SaveMilestone(string name)
    {
        PlayerPrefs.SetInt(name, 1);
        PlayerPrefs.Save();
    }

    public  bool GetMilestoneCompleted(string name)
    {
        return PlayerPrefs.GetInt(name, 0) == 1;
    }

    public static void ResetMilestones()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
