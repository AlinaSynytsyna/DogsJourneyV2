using UnityEngine;
using UnityEngine.SceneManagement;

public class Zima : Player
{
    Collider2D[] Colliders;
    public bool IsDashing;
    System.Random AnimationState = new System.Random();
    protected override void Awake()
    {
        Name = "Zima";
        //Self = GetComponent<NPC>();
        Info = FindObjectOfType<LevelInfo>();
        //Runner = GetComponentInChildren<DialogueRunner>();
        PlayerCamera = FindObjectOfType<PlayerCamera>();
        Rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Colliders = GetComponents<Collider2D>();
        if (Info.CheckCharacter("Zima"))
        {
            enabled = true;
            Health = 100;
            Speed = 0F;
            JumpForce = 7F;
            CheckCharacter();
            if (SaveLoadSystem.HasInfo && SaveLoadSystem.CurrentSceneIndex == SceneManager.GetActiveScene().buildIndex)
            {
                transform.position = SaveLoadSystem.ZimaPosition;
                Health = SaveLoadSystem.ZimaHealth;
            }
        }
        else
        {
            foreach (Collider2D Col in Colliders)
                Col.enabled = false;
            Rigid.isKinematic = true;
            //Self.talkToNode = "Zima";
            //Self.enabled = true;
            PlayerCamera.gameObject.SetActive(false);
            //Runner.startAutomatically = false;
            //Runner.enabled = false;
            IsActive = false;
            enabled = false;
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
        }
    }
    private void FixedUpdate()
    {
        if (enabled)
        {
            CheckCharacter();
            if (IsActive)
            {
                GroundCheck();
                HeightCheck();
                FallHealthCheck();
                CheckHealth();
                IdleCount();
            }
        }
    }
    private void Update()
    {
        if (IsActive)
        {
            //if (Runner.IsDialogueRunning == true)
            //{
            //    Animator.SetBool("IsJumping", false);
            //    Animator.SetBool("IsFalling", false);
            //    Animator.SetFloat("Speed", 0);
            //    return;
            //}
            if (GroundCheck())
            {
                IsDashing = false;
                IsJumping = false;
                Animator.SetBool("IsJumping", false);
                Animator.SetBool("IsFalling", false);
                Height = 0;
                if (!Input.GetKey(_customInput.Left) || !Input.GetKey(_customInput.Right))
                {
                    Speed = 0;
                    IsWalking = false;
                    Animator.SetFloat("Speed", 0);
                }
                if (Input.GetKeyDown(_customInput.Jump)) Jump();
            }
            if (Input.GetKey(_customInput.Left) || Input.GetKey(_customInput.Right))
                Walk();
            if (Input.GetKeyDown(_customInput.ChangeCharacter) && CountPlayers() > 1)
            {
                LevelInfo Info = FindObjectOfType<LevelInfo>();
                if (Info.CheckCharacter("Zima"))
                    CharacterChanger.SwitchCharacter();
            }
            if (Input.GetKeyDown(_customInput.SpecialAbility) && !IsDashing)
                SpecialAbility();
        }
        else
        {
            Animator.SetBool("IsJumping", false);
            Animator.SetBool("IsFalling", false);
            Animator.SetFloat("Speed", 0);
            return;
        }
    }
    public override void Walk()
    {
        Speed = 3.5F;
        float Axis = 0;
        if (Input.GetKey(_customInput.Left))
            Axis = -1;
        else if (Input.GetKey(_customInput.Right))
            Axis = 1;
        if (GroundCheck())
            Animator.SetFloat("Speed", Speed);
        Vector3 Direction = transform.right * Axis;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Direction, Speed * Time.deltaTime);
        Renderer.flipX = Direction.x > 0;
        IsWalking = true;
    }

    public override void Jump()
    {
        IsJumping = true;
        Animator.SetBool("IsJumping", true);
        Rigid.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    private void IdleCount()
    {
        if (Input.anyKey)
            Timer = 0;
        else
        {
            Timer += Time.deltaTime;
            if (Timer > 15)
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
                Timer = 0;
            }
        }
    }

    public void FallHealthCheck()
    {
        if (OnGround && Height > 50)
        {
            Health -= (Height - 50) / 5;
        }
    }

    public void CheckHealth()
    {
        if (Health <= 0)
        {
            Health = 0;
            Animator.Play("Death");
            Invoke("Reload", 3F);
            enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D Entity)
    {
        if (Entity.tag == "DeathTrigger")
            Health = 0;
    }

    public override void SpecialAbility()
    {
        if (IsJumping)
        {
            if (Renderer.flipX)
                Rigid.velocity = Vector2.right * 6;
            else if (!Renderer.flipX)
                Rigid.velocity = Vector2.left * 6;
            IsDashing = true;
        }
    }

    public override void CheckCharacter()
    {
        if (CharacterChanger.ActiveCharacter == 0)
        {
            IsActive = true;
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            foreach (Collider2D Col in Colliders)
                Col.enabled = true;
            //Self.talkToNode = "";
            Rigid.isKinematic = false;
            PlayerCamera.gameObject.SetActive(true);
        }
        else
        {
            IsActive = false;
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
            foreach (Collider2D Col in Colliders)
                Col.enabled = false;
            //Self.talkToNode = "";
            Rigid.isKinematic = true;
            PlayerCamera.gameObject.SetActive(false);

        }
    }

    public override bool GroundCheck()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4F, transform.position.y - 0.5F), new Vector2(transform.position.x + 0.4F, transform.position.y - 0.9F), GroundLayer);
    }
}