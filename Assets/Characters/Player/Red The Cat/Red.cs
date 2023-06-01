using System.Linq;
using UnityEngine;

public class Red : Player
{
    public override void CheckFallDamage()
    {
        if (IsOnTheGround())
        {
            Health = Height > 100 ? Health - (Height - 100) / 5 : Health;
            Height = 0;
        }
    }

    public override void UseSpecialAbility()
    {
        if (!IsOnTheGround())
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
            Rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            IsUsingSpecialAbility = true;
        }
    }

    public void RedIntroDialogueComplete()
    {
        VariableStorage.TryGetValue(YarnConstants.PipeOpen, out bool isDialogueHappened);

        if (isDialogueHappened)
        {
            FindObjectsOfType<DialogueTrigger>().Where(x => x.name.Equals(Constants.PipeDialogue)).First().IsActive = false;
            FindObjectsOfType<TeleportationTrigger>().Where(x => x.name.Equals(Constants.NextLevelTeleport)).First().IsActive = true;
        }
    }

    public void RedSewersDialogueComplete()
    {
        VariableStorage.TryGetValue(YarnConstants.RedPlayable, out bool isDialogueHappened);

        if (isDialogueHappened)
        {
            GetComponentInChildren<DialogueTrigger>().YarnScriptForZima = YarnConstants.RedGenericDialogue;
            IsPlayableInScene = true;
            FindObjectsOfType<TutorialTrigger>().All(x => x.IsActive = true);
            GetComponentInChildren<DialogueTrigger>().OnDialogueEndedAction = null;
        }
    }
}