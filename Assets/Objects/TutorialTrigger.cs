using Knot.Localization;
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

        IsActive = TutorialTriggersManager.GetIsTutorialTriggerActive(Id);

        CheckText();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag) && IsActive)
        {
            CheckText();
            _tutorialCanvas.worldCamera = FindObjectOfType<PlayerCamera>().GetCamera();
            _tutorialCanvas.gameObject.SetActive(true);
        }
    }

    public new void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag) && IsActive)
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
                TutorialText.text = string.Format(KnotLocalization.GetText(LocalizationConstants.TutorialMovingText), CustomInput.Left, CustomInput.Right, CustomInput.Jump);
                break;
            case TutorialType.UsingSpecialAbilityZima:
                TutorialText.text = string.Format(KnotLocalization.GetText(LocalizationConstants.TutorialUsingSpecialAbilityZimaText), CustomInput.SpecialAbility);
                break;
            case TutorialType.UsingSpecialAbilityRed:
                TutorialText.text = string.Format(KnotLocalization.GetText(LocalizationConstants.TutorialUsingSpecialAbilityRedText), CustomInput.SpecialAbility);
                break;
            case (TutorialType.InteractionWithEnvironment):
                TutorialText.text = string.Format(KnotLocalization.GetText(LocalizationConstants.TutorialInteractText), CustomInput.Interact);
                break;
            case (TutorialType.Healing):
                TutorialText.text = KnotLocalization.GetText(LocalizationConstants.TutorialHealingText);
                break;
            case (TutorialType.ChangeBetweenCharacters):
                TutorialText.text = string.Format(KnotLocalization.GetText(LocalizationConstants.TutorialChangeCharacterText), CustomInput.ChangeCharacter);
                break;
            case (TutorialType.SaveProgress):
                TutorialText.text = KnotLocalization.GetText(LocalizationConstants.TutorialSaveProgressText);
                break;
        }
    }
}
