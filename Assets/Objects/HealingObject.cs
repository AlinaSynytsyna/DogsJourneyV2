using UnityEngine;

public class HealingObject : BaseTrigger
{
    public string HealingObjectId;

    public new void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Trigger = GetComponent<CircleCollider2D>();
        CheckIfHealingObjectIsDestroyed();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag) && IsActive)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer.Health < 100 && HealingObjectsManager.IsHealingObjectActive(HealingObjectId))
            {
                HealingObjectsManager.MarkHealingObjectAsDestroyed(HealingObjectId);
                ActivePlayer.Health += 10;

                if (ActivePlayer.Health > 100)
                    ActivePlayer.Health = 100;

                Renderer.enabled = false;
                Trigger.enabled = false;

                IsActive = false;
            }
        }
    }

    public new void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag))
        {
            ActivePlayer = null;
        }
    }

    public void CheckIfHealingObjectIsDestroyed()
    {
        IsActive = HealingObjectsManager.IsHealingObjectActive(HealingObjectId);
        Debug.Log(IsActive);
        Renderer.enabled = IsActive;
        Trigger.enabled = IsActive;
    }
}