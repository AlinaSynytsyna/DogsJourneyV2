using UnityEngine;

public class Zima : Player
{
    System.Random AnimationState = new System.Random();

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        CountIdleTimer();
    }

    public override void CheckFallDamage()
    {
        if (IsOnTheGround())
        {
            Health = Height > 75 ? Health - (Height - 75) / 5 : Health;
            Height = 0;
        }
    }


    public override void UseSpecialAbility()
    {
        if (!IsOnTheGround())
        {
            Rigidbody.velocity = Renderer.flipX ? Rigidbody.velocity = Vector2.right * 6 : Rigidbody.velocity = Vector2.left * 6;

            IsUsingSpecialAbility = true;
        }
    }

    public void CountIdleTimer()
    {
        if (Input.anyKey)
            IdleTimer = 0;
        else
        {
            IdleTimer += Time.deltaTime;
            if (IdleTimer > 8)
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