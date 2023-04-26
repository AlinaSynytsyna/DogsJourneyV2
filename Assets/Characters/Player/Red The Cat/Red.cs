using UnityEngine;

public class Red : Player
{
    public override void CheckFallDamage()
    {
        if (IsOnTheGround() && Height > 90)
        {
            Health -= (Height - 90) / 4;
        }
    }

    public override void UseSpecialAbility()
    {
        if (!IsOnTheGround())
        {
            Rigidbody.AddForce(transform.up * (JumpForce * 0.7f), ForceMode2D.Impulse);
            IsUsingSpecialAbility = true;
        }
    }
}
