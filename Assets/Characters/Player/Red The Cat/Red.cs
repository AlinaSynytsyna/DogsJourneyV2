using UnityEngine;
using UnityEngine.SceneManagement;

public class Red : Player
{
    Collider2D[] Colliders;
    public bool IsDoubleJumping;
    protected override void Awake()
    {
        Name = "Red";
        //Self = GetComponent<NPC>();
        Info = FindObjectOfType<LevelInfo>();
        //Runner = GetComponentInChildren<DialogueRunner>();
        PlayerCamera = GetComponentInChildren<PlayerCamera>();
        Rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Colliders = GetComponents<Collider2D>();
        if (Info.CheckCharacter("Red"))
        {
            enabled = true;
            Health = 100;
            Speed = 0F;
            JumpForce = 7F;
            CheckCharacter();
            if (SaveLoadSystem.HasInfo && SaveLoadSystem.CurrentSceneIndex == SceneManager.GetActiveScene().buildIndex)
            {
                transform.position = SaveLoadSystem.RedPosition;
                Health = SaveLoadSystem.RedHealth;
            }
        }
        else
        {
            foreach (Collider2D Col in Colliders)
                Col.enabled = false;
            Rigid.isKinematic = true;
            //if (SceneManager.GetActiveScene().name == "StreetScene01")
            //    Self.talkToNode = "Red";
            //if (SceneManager.GetActiveScene().name == "StreetScene02")
            //    Self.talkToNode = "RedSewersIntro";
            //Self.enabled = true;
            //PlayerCamera.gameObject.SetActive(false);
            //Runner.startAutomatically = false;
            //Runner.enabled = false;
            IsActive = false;
            enabled = false;
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
        }
    }
    public void FixedUpdate()
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
            }
        }
    }
    public void Update()
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
                IsJumping = false;
                IsDoubleJumping = false;
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
                CharacterChanger.SwitchCharacter();
            if (Input.GetKeyDown(_customInput.SpecialAbility) && !IsDoubleJumping)
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

    public override bool GroundCheck()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4F, transform.position.y - 0.5F), new Vector2(transform.position.x + 0.4F, transform.position.y - 0.9F), GroundLayer);
    }

    public override void Walk()
    {
        Speed = 3.2F;
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
    public void FallHealthCheck()
    {
        if (OnGround && Height > 90)
        {
            Health -= (Height - 90) / 4;
        }
    }
    public void CheckHealth()
    {
        if (Health <= 0)
        {
            Health = 0;
            Animator.Play("Red_Death");
            Invoke("Reload", 3F);
            this.enabled = false;
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
            Rigid.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            IsDoubleJumping = true;
        }
    }
    public override void CheckCharacter()
    {
        {
            if (CharacterChanger.ActiveCharacter == 1)
            {
                IsActive = true;
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                foreach (Collider2D Col in Colliders)
                    Col.enabled = true;
                Rigid.isKinematic = false;
                //Self.talkToNode = "";
                //Self.enabled = false;
                PlayerCamera.gameObject.SetActive(true);
            }
            else
            {
                IsActive = false;
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
                foreach (Collider2D Col in Colliders)
                    Col.enabled = false;
                Rigid.isKinematic = true;
                //if (SceneManager.GetActiveScene().name == "StreetScene01")
                //    Self.talkToNode = "Red";
                //if (SceneManager.GetActiveScene().name == "StreetScene02")
                //    Self.talkToNode = "RedSewersIntro";
                //Self.enabled = true;
                PlayerCamera.gameObject.SetActive(false);
            }
        }
    }
}
