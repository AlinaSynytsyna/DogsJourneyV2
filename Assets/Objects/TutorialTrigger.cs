using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : BaseTrigger
{
    public enum TutorialType { Moving, InteractionWithEnvironment, UsingSpecialAbilityZima, UsingSpecialAbilityRed, Healing, ChangeBetweenCharacters, SaveProgress }

    public Text TutorialText;
    public TutorialType State;
    private Canvas _tutorialCanvas;

    public new void Awake()
    {
        CustomInput = CustomInputManager.GetCustomInputKeys();
        Trigger = GetComponent<CircleCollider2D>();
        TutorialText = GetComponentInChildren<Text>();
        _tutorialCanvas = GetComponentInChildren<Canvas>();
        _tutorialCanvas.gameObject.SetActive(false);
        CheckText();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == EntityTagPlayer)
        {
            _tutorialCanvas.worldCamera = FindObjectOfType<PlayerCamera>().GetCamera();
            _tutorialCanvas.gameObject.SetActive(true);
        }
    }

    public new void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == EntityTagPlayer)
        {
            ActivePlayer = null;
            _tutorialCanvas.worldCamera = null;
            _tutorialCanvas.gameObject.SetActive(false);
        }
    }

    public void CheckText()
    {
        switch (State)
        {
            case TutorialType.Moving:
                TutorialText.text = $"Управление персонажем: \nВлево: {CustomInput.Left}\nВправо: {CustomInput.Right}\nПрыжок: {CustomInput.Jump}";
                break;
            case TutorialType.UsingSpecialAbilityZima:
                TutorialText.text = $"Зима может перепрыгивать длинные расстояния по нажатию клавиши {CustomInput.SpecialAbility}";
                break;
            case TutorialType.UsingSpecialAbilityRed:
                TutorialText.text = $"Рэд может совершать двойной прыжок по нажатию клавиши {CustomInput.SpecialAbility}";
                break;
            case (TutorialType.InteractionWithEnvironment):
                TutorialText.text = $"Взаимодействовать с окружением возможно по нажатии клавиши {CustomInput.Interact}";
                break;
            case (TutorialType.Healing):
                TutorialText.text = $"Пончик восстанавливает здоровье на 10 единиц";
                break;
            case (TutorialType.ChangeBetweenCharacters):
                TutorialText.text = $"Между персонажами можно переключаться по нажатию клавиши {CustomInput.ChangeCharacter}";
                break;
            case (TutorialType.SaveProgress):
                TutorialText.text = $"Прогресс можно сохранять в специальных местах, отмеченных крестом";
                break;
        }
    }
}
