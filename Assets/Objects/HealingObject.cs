using UnityEngine;

public class HealingObject : BaseTrigger
{
    public new void Awake()
    {
        Trigger = GetComponent<CircleCollider2D>();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == EntityTagPlayer)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer.Health < 100)
            {
                ActivePlayer.Health += 10;

                if (ActivePlayer.Health > 100)
                    ActivePlayer.Health = 100;

                Destroy(gameObject, 0.2f);
            }
        }
    }
}
