using UnityEngine;

public class Red : Player
{
    protected override void Awake()
    {
        base.Awake();

        if (LevelInfo.CheckIfTheCharacterIsPlayable("Red"))
        {
            enabled = true;
            Health = 100;
            Speed = 0F;
            JumpForce = 7F;
            CheckCharacter();
        }
        else
        {
            Rigidbody.isKinematic = true;
            IsPlayerActive = false;
            enabled = false;
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
        }
    }

    public void FixedUpdate()
    {
        if (enabled)
        {
            CheckCharacter();
            if (IsPlayerActive)
            {
                IsOnTheGround();
                CheckHeight();
                FallHealthCheck();
                CheckHealth();
            }
        }
    }

    public void Update()
    {
        if (IsPlayerActive)
        {
            if (IsOnTheGround())
            {
                Animator.SetBool("IsJumping", false);
                Animator.SetBool("IsFalling", false);
                Height = 0;

                if (!Input.GetKey(CustomInput.Left) || !Input.GetKey(CustomInput.Right))
                {
                    Speed = 0;
                    IsWalking = false;
                    Animator.SetFloat("Speed", 0);
                }

                if (Input.GetKeyDown(CustomInput.Jump)) Jump();
            }

            if (Input.GetKey(CustomInput.Left) || Input.GetKey(CustomInput.Right))
                Walk();

            if (Input.GetKeyDown(CustomInput.ChangeCharacter))
                CharacterChanger.SwitchCharacter();

            if (Input.GetKeyDown(CustomInput.SpecialAbility) && !IsUsingSpecialAbility)
                UseSpecialAbility();
        }
        else
        {
            Animator.SetBool("IsJumping", false);
            Animator.SetBool("IsFalling", false);
            Animator.SetFloat("Speed", 0);
            return;
        }
    }

    public override bool IsOnTheGround()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4F, transform.position.y - 0.5F), new Vector2(transform.position.x + 0.4F, transform.position.y - 0.9F), GroundLayerMask);
    }

    public override void FallHealthCheck()
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
            Rigidbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            IsUsingSpecialAbility = true;
        }
    }
    public void CheckCharacter()
    {
        {
            if (CharacterChanger.ActiveCharacter == 1)
            {
                IsPlayerActive = true;
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                Rigidbody.isKinematic = false;
            }
            else
            {
                IsPlayerActive = false;
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
                Rigidbody.isKinematic = true;
            }
        }
    }
}
