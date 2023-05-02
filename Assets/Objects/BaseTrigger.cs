using System.Linq;
using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{
    public Player ActivePlayer;
    protected const string PlayerEntityTag = nameof(Player);
    protected CustomInput CustomInput;
    protected CircleCollider2D Trigger;
    protected SpriteRenderer Renderer;

    private const int _triggerInteractRadiusMultiplier = 4;

    public void Awake()
    {
        CustomInput = CustomInputManager.GetCustomInputKeys();
        Trigger = GetComponent<CircleCollider2D>();

        Renderer = GetComponentInChildren<SpriteRenderer>();
        Renderer.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer != null)
                Renderer.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            ActivePlayer = null;
            Renderer.gameObject.SetActive(false);
        }
    }

    public Player FindActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => (x.transform.position - transform.position).magnitude <= Trigger.radius * _triggerInteractRadiusMultiplier && x.IsPlayerActive).First();
    }
}

