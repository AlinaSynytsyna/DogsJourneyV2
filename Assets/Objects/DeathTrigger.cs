using System.Linq;
using UnityEngine;


public class DeathTrigger : MonoBehaviour
{
    private const string PlayerEntityTag = nameof(Player);
    private Player _activePlayer;
    private Collider2D _trigger;

    public void Awake()
    {
        _trigger = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            _activePlayer = FindActivePlayer();
            _activePlayer.Health = 0;
        }
    }

    public void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag)
        {
            _activePlayer = null;
        }
    }

    public Player FindActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => x.IsPlayerActive).First();
    }
}

