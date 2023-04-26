using UnityEngine;

public class Zima : Player
{
    System.Random AnimationState = new System.Random();

    public override void CheckFallDamage()
    {
        if (IsOnTheGround() && Height > 50)
        {
            Health -= (Height - 50) / 4;
        }
    }


    public override void UseSpecialAbility()
    {
        if (!IsOnTheGround())
        {
            Rigidbody.velocity = Renderer.flipX ? Rigidbody.velocity = Vector2.right * 5 : Rigidbody.velocity = Vector2.left * 5;

            IsUsingSpecialAbility = true;
        }
    }

    private void CountIdleTimer()
    {
        if (Input.anyKey)
            IdleTimer = 0;
        else
        {
            IdleTimer += Time.deltaTime;
            if (IdleTimer > 15)
            {
                IdleState = AnimationState.Next(1, 3);
                switch (IdleState)
                {
                    case 1:
                        Animator.Play("Idle_yawn");
                        break;
                    case 2:
                        Animator.Play("Idle_standing");
                        break;
                }
                IdleTimer = 0;
            }
        }
    }
}