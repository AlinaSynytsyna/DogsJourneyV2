using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Color _zimaGUIColor = Color.HSVToRGB(0.95F, 0.7F, 1F);
    private Color _redGUIColor = Color.HSVToRGB(0.5F, 0.6F, 1F);

    private Player _player;
    private Text _healthText;

    public void Start()
    {
        _healthText = GetComponentInChildren<Text>();
        _player = FindObjectsOfType<Player>().Where(x => x.IsActive).First();
        _healthText.color = _player is Zima ? _zimaGUIColor : _redGUIColor;
    }

    public void LateUpdate()
    {
        _healthText.text = "Здоровье: " + _player.Health;
    }

    public void ChangeGUIColor()
    {
        _healthText.color = _player is Zima ? _zimaGUIColor : _redGUIColor;
    }
}
