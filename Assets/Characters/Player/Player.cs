using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    public string PlayerName;
    public float Speed;
    public int Health = 100;
    public float JumpForce;
    public bool IsPlayerActive = false;
    public bool IsUsingSpecialAbility = false;
    public bool IsWalking = false;
    public bool IsTalking;
    public LayerMask GroundLayerMask;
    public int Height = 0;

    protected CustomInput CustomInput;
    protected Rigidbody2D Rigidbody;
    protected Animator Animator;
    protected SpriteRenderer Renderer;
    protected Collider2D Collider;
    protected LevelInfo LevelInfo;
    protected float IdleTimer = 0;
    protected int IdleState = 0;
    protected DialogueRunTrigger DialogueRunTrigger;
    protected IEnumerator Enumerator;

    public abstract void UseSpecialAbility();

    public abstract void CheckFallDamage();

    protected void Awake()
    {
        enabled = true;
        LevelInfo = FindObjectOfType<LevelInfo>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
        DialogueRunTrigger = GetComponentInChildren<DialogueRunTrigger>();
        CustomInput = CustomInputManager.GetCustomInputKeys();

        if (!LevelInfo.CheckIfTheCharacterIsPlayable(this))
        {
            MarkPlayerAsUnplayable();
            enabled = false;
        }
        else if (LevelInfo.MainPlayableCharacter == PlayerName && LevelInfo.ActivePlayer == null)
        {
            MarkPlayerAsPlayable();
            LevelInfo.ActivePlayer = this;
        }

        LoadDataFromFile();
    }

    private void FixedUpdate()
    {
        if (IsPlayerActive)
        {
            IsOnTheGround();
            CheckFallDamage();
            CheckPlayerHealth();
            CheckFallHeight();
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
                IsWalking = false;
                Animator.SetFloat("Speed", 0);

                if (Input.GetKeyDown(CustomInput.Jump))
                    Jump();
            }

            if (Input.GetKey(CustomInput.Left) || Input.GetKey(CustomInput.Right))
                Walk();

            if (Input.GetKeyDown(CustomInput.SpecialAbility) && !IsUsingSpecialAbility)
                UseSpecialAbility();
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadDataFromFile()
    {
        if (LevelManager.HasInformation && !LevelManager.IsReloadingLevel)
        {
            var playerStats = LevelManager.GetPlayerStats()[PlayerName];
            Health = playerStats.Health;
            transform.position = new Vector2(playerStats.PositionX, playerStats.PositionY);
            IsPlayerActive = playerStats.IsActive;

            if (IsPlayerActive)
            {
                MarkPlayerAsPlayable();
                LevelInfo.ActivePlayer = this;
            }

            else MarkPlayerAsUnplayable();
        }
    }

    public void Walk()
    {
        float axis = 0;
        if (Input.GetKey(CustomInput.Left))
            axis = -1;
        else if (Input.GetKey(CustomInput.Right))
            axis = 1;

        if (IsOnTheGround())
            Animator.SetFloat("Speed", Speed);

        Vector3 direction = transform.right * axis;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Speed * Time.deltaTime);
        Renderer.flipX = direction.x > 0;
        IsWalking = true;
    }

    public void Jump()
    {
        Animator.SetBool("IsJumping", true);
        Rigidbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    protected void CheckFallHeight()
    {
        if (Rigidbody.velocity.y < -0.1)
        {
            Height += 1;
            Animator.SetBool("IsFalling", true);
        }
    }

    public void CheckPlayerHealth()
    {
        if (Health <= 0)
        {
            Health = 0;
            Animator.Play("Death");
            Invoke(nameof(Reload), 3F);
            enabled = false;
        }
    }

    public bool IsOnTheGround()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.4f, transform.position.y - 1f), GroundLayerMask);
    }

    public void MarkPlayerAsUnplayable()
    {
        IsPlayerActive = false;
        transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
        DialogueRunTrigger.gameObject.SetActive(true);

        Enumerator = WaitForPlayerToLand();
        StartCoroutine(Enumerator);
    }

    public void MarkPlayerAsPlayable()
    {
        IsPlayerActive = true;
        Collider.enabled = true;
        Rigidbody.isKinematic = false;
        transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        DialogueRunTrigger.gameObject.SetActive(false);
    }

    public void SwitchPlayerComponentsOff()
    {
        Animator.SetBool("IsJumping", false);
        Animator.SetBool("IsFalling", false);
        Animator.SetFloat("Speed", 0);

        Collider.enabled = false;
        Rigidbody.isKinematic = true;
        StopCoroutine(Enumerator);
    }

    private IEnumerator WaitForPlayerToLand()
    {
        while (true)
        {
            yield return null;
            if (IsOnTheGround())
            {
                Invoke(nameof(SwitchPlayerComponentsOff), 0.3f);
            }
        }
    }
}

