using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Zima Zima;
    private Red Red;
    private Color ZimaColor = Color.HSVToRGB(0.95F, 0.7F, 1F);
    private Color RedColor = Color.HSVToRGB(0.5F, 0.6F, 1F);
    private Player Player;
    private Text HealthText;
    public void Awake()
    {
        HealthText = GetComponentInChildren<Text>();
        Player = GetComponentInParent<PlayerCamera>().GetActivePlayer();
        if (Player is Zima)
            ZimaHUD();
        if (Player is Red)
            RedHUD();
    }
    public void Update()
    {
        if (Player is Zima)
            ZimaHUD();
        if (Player is Red)
            RedHUD();
    }
    public void ZimaHUD()
    {
        Zima = GetComponentInParent<Zima>();
        HealthText.color = ZimaColor;
        HealthText.text = "Здоровье: " + (Zima.Health);
    }
    public void RedHUD()
    {
        Red = GetComponentInParent<Red>();
        HealthText.color = RedColor;
        HealthText.text = "Здоровье: " + (Red.Health);
    }
}
