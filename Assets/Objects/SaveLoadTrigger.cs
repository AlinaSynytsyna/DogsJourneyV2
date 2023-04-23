using UnityEngine;

public class SaveLoadTrigger : BaseTrigger
{

    public void OnTriggerStay2D(Collider2D entity)
    {
        if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact))
        {
        }
    }
}