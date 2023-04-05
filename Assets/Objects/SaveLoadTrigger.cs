using UnityEngine;

public class SaveLoadTrigger : BaseTrigger
{
    public SaveLoadSystem SaveLoadSystem;

    public void OnTriggerStay2D(Collider2D entity)
    {
        if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact))
        {
            Debug.Log("saved progress");
            SaveLoadSystem.SavePlayerInfo(ActivePlayer);
            SaveLoadSystem.SaveCurrentScene();
        }
    }
}