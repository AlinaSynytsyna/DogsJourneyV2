using UnityEngine;

public class Red : Player
{
    public override void CheckFallDamage()
    {
        if (IsOnTheGround())
        {
            Health = Height > 120 ? Health - (Height - 120) / 5 : Health;
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
}
