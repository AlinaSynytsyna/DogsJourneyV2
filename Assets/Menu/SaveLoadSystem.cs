using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
[CreateAssetMenu(fileName = "SaveLoadSystem", menuName = "Save-Load System", order = 54)]
public class SaveLoadSystem : ScriptableObject
{
    public bool HasInfo;
    public CharacterChanger Changer;
    public int CurrentSceneIndex;
    public Vector3 ZimaPosition;
    public int ZimaHealth;
    public Vector3 RedPosition;
    public int RedHealth;
    public void SavePlayerInfo(Player Player)
    {
        if (Player is Zima)
        {
            ZimaPosition = Player.transform.position;
            ZimaHealth = Player.Health;
        }
        if (Player is Red)
        {
            RedPosition = Player.transform.position;
            RedHealth = Player.Health;
        }
        HasInfo = true;
    }
    public void SaveCurrentScene()
    {
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        HasInfo = true;
    }
    public void CleanAllData()
    {
        HasInfo = false;
        Changer.ActiveCharacter = 0;
    }
}
