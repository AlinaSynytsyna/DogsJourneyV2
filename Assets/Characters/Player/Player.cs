using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    public float Speed;
    public int Health = 100;
    public string PlayerName;
    public bool IsPlayerActive = false;
    public bool IsUsingSpecialAbility = false;
    public bool IsWalking = false;
    public CharacterChanger CharacterChanger;
    public LayerMask GroundLayerMask;
    public int Height = 0;

    protected CustomInput CustomInput;
    protected Rigidbody2D Rigidbody;
    protected Animator Animator;
    protected SpriteRenderer Renderer;
    protected Collider2D Collider;
    protected LevelInfo LevelInfo;
    protected float JumpForce;
    protected float IdleTimer = 0;
    protected int IdleState = 0;

    public abstract bool IsOnTheGround();

    public abstract void UseSpecialAbility();

    public abstract void FallHealthCheck();

    protected virtual void Awake()
    {
        enabled = true;
        LevelInfo = FindObjectOfType<LevelInfo>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
        CustomInput = CustomInputManager.GetCustomInputKeys();
        JumpForce = 7F;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, Speed * Time.deltaTime);
        Renderer.flipX = direction.x > 0;
        IsWalking = true;
    }

    public void Jump()
    {
        Animator.SetBool("IsJumping", true);
        Rigidbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    protected void CheckHeight()
    {
        if (Rigidbody.velocity.y < -0.1)
        {
            Height += 1;
            Animator.SetBool("IsFalling", true);
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

    public void PlayerStartedDialogue()
    {
        IsPlayerActive = false;
        Rigidbody.isKinematic = true;
        Animator.SetBool("IsJumping", false);
        Animator.SetBool("IsFalling", false);
        Animator.SetFloat("Speed", 0);
    }

    public void PlayerEndedDialogue()
    {
        IsPlayerActive = true;
        Rigidbody.isKinematic = false;
    }
}
