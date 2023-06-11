using UnityEngine;

public class HealingObject : BaseTrigger
{
    public new void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Trigger = GetComponent<CircleCollider2D>();
        CheckIfHealingObjectIsActive();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag) && IsActive)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer.Health < 100 && HealingObjectsManager.IsHealingObjectActive(Id))
            {
                HealingObjectsManager.MarkHealingObjectAsDestroyed(Id);
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

    public void CheckIfHealingObjectIsActive()
    {
        IsActive = HealingObjectsManager.IsHealingObjectActive(Id);
        Renderer.enabled = IsActive;
        Trigger.enabled = IsActive;
    }
}