using System.Linq;
using UnityEngine;


public abstract class BaseTrigger : MonoBehaviour
{
    protected const string EntityTagPlayer = nameof(Player);
    protected Player ActivePlayer;
    protected CustomInput CustomInput;
    protected CircleCollider2D Trigger;
    private SpriteRenderer _renderer;
    private const int _triggerInteractRadius = 3;

    public void Awake()
    {
        Trigger = GetComponent<CircleCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.gameObject.SetActive(false);
        CustomInput = CustomInputManager.GetCustomInputKeys();
    }

    public void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == EntityTagPlayer)
        {
            ActivePlayer = FindActivePlayer();

            _renderer.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == EntityTagPlayer)
        {
            ActivePlayer = null;
            _renderer.gameObject.SetActive(false);
        }
    }

    public Player FindActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => (x.transform.position - transform.position).magnitude <= Trigger.radius * _triggerInteractRadius && x.IsActive).First();
    }
}

