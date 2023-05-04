using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Player ActivePlayer;

    private Color _zimaHUDColor = Color.HSVToRGB(0.95F, 0.7F, 1F);
    private Color _redHUDColor = Color.HSVToRGB(0.5F, 0.6F, 1F);
    private Text _healthText;

    public void Start()
    {
        _healthText = GetComponentInChildren<Text>();
        ActivePlayer = FindObjectsOfType<Player>().Where(x => x.IsPlayerActive).First();
        _healthText.color = ActivePlayer is Zima ? _zimaHUDColor : _redHUDColor;
    }

    public void LateUpdate()
    {
        _healthText.text = "Здоровье: " + ActivePlayer.Health;
    }

    public void ChangeGUIColor()
    {
        _healthText.color = ActivePlayer is Zima ? _zimaHUDColor : _redHUDColor;
    }
}
