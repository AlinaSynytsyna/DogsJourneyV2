using UnityEngine;

public class Zima : Player
{
    System.Random AnimationState = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        if (!LevelInfo.CheckIfTheCharacterIsPlayable(PlayerName))
        {
            enabled = false;
            Rigidbody.isKinematic = true;
            IsPlayerActive = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }
        else if (LevelInfo.MainPlayableCharacter == PlayerName)
        {
            IsPlayerActive = true;
        }
    }

    public void OnEnable()
    {
        if (LevelManager.HasInformation && !LevelManager.IsReloadingLevel)
        {
            var zimaStats = LevelManager.GetPlayerStats()[PlayerName];
            Health = zimaStats.Health;
            transform.position = new Vector2(zimaStats.PositionX, zimaStats.PositionY);
            IsPlayerActive = zimaStats.IsActive;
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerActive)
        {
            IsOnTheGround();
            CheckHeight();
            FallHealthCheck();
            CheckHealth();
            CountIdleTimer();
        }
    }

    private void Update()
    {
        if (IsPlayerActive)
        {
            if (IsOnTheGround())
            {
                IsUsingSpecialAbility = false;
                Animator.SetBool("IsJumping", false);
                Animator.SetBool("IsFalling", false);
                Height = 0;

                if (!Input.GetKey(CustomInput.Left) || !Input.GetKey(CustomInput.Right))
                {
                    IsWalking = false;
                    Animator.SetFloat("Speed", 0);
                }

                if (Input.GetKeyDown(CustomInput.Jump)) Jump();
            }

            if (Input.GetKey(CustomInput.Left) || Input.GetKey(CustomInput.Right)) Walk();

            if (Input.GetKeyDown(CustomInput.ChangeCharacter))
            {
                if (LevelInfo.CheckIfTheCharacterIsPlayable("Zima"))
                    CharacterChanger.SwitchCharacter();
            }

            if (Input.GetKeyDown(CustomInput.SpecialAbility) && !IsUsingSpecialAbility)
                UseSpecialAbility();
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

    public override void FallHealthCheck()
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

    public override bool IsOnTheGround()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.4f, transform.position.y - 0.6f), GroundLayerMask);
    }
}