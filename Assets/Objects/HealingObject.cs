using UnityEngine;

public class HealingObject : BaseTrigger
{
    public string HealingObjectId;

    public new void Awake()
    {
        Trigger = GetComponent<CircleCollider2D>();
        Invoke(nameof(CheckIfHealingObjectIsDestroyed), 0.2f);
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer.Health < 100)
            {
                ActivePlayer.Health += 10;

                if (ActivePlayer.Health > 100)
                    ActivePlayer.Health = 100;

                HealingObjectsManager.MarkHealingObjectAsDestroyed(HealingObjectId);
                Destroy(gameObject, 0.15f);
            }
        }
    }

    public new void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            ActivePlayer = null;
        }
    }

    public void CheckIfHealingObjectIsDestroyed()
    {
        if(!HealingObjectsManager.IsHealingObjectActive(HealingObjectId))
        {
            Destroy(gameObject);
        }
    }
}