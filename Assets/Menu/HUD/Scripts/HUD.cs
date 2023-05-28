using Knot.Localization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Player ActivePlayer;

    private Color _zimaHUDColor = Color.HSVToRGB(0.9F, 0.6F, 1F);
    public Color _redHUDColor = Color.HSVToRGB(0.6F, 0.5F, 1F);
    private Text _healthText;
    private string _healthString;

    public void Start()
    {
        _healthText = GetComponentInChildren<Text>();
        ActivePlayer = FindObjectsOfType<Player>().Where(x => x.IsPlayerActive).First();

        _healthText.color = ActivePlayer is Zima ? _zimaHUDColor : _redHUDColor;

        KnotLocalization.RegisterTextUpdatedCallback(LocalizationConstants.HUDHealthText, HUDHealthTextUpdated);

        _healthString = KnotLocalization.GetText(LocalizationConstants.HUDHealthText);
    }

    public void LateUpdate()
    {
        _healthText.text = $"{_healthString}: " + ActivePlayer.Health;
    }

    public void ChangeGUIColor()
    {
        _healthText.color = ActivePlayer is Zima ? _zimaHUDColor : _redHUDColor;
    }

    private void HUDHealthTextUpdated(string text)
    {
        if (!_healthText.IsDestroyed())
            _healthText.text = $"{text}: " + ActivePlayer.Health;
    }
}
