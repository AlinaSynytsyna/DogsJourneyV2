using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    protected CustomInput _customInput;
    protected Rigidbody2D Rigidbody;
    protected Animator Animator;
    protected SpriteRenderer Renderer;

    [SerializeField]
    protected SaveLoadSystem SaveLoadSystem;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public int Health = 100;
    [SerializeField]
    protected float JumpForce;
    protected Collider2D Collider;
    protected string Name;
    protected LevelInfo Info;
    public bool IsActive = false;
    public CharacterChanger CharacterChanger;
    public bool OnGround;
    public LayerMask GroundLayer;
    public bool IsWalking;
    protected float Timer = 0;
    protected bool IsJumping = false;
    protected int IdleState = 0;
    public int Height = 0;

    public abstract bool GroundCheck();

    protected virtual void Awake()
    {
        enabled = true;
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
        _customInput = CustomInputManager.GetCustomInputKeys();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public abstract void Walk();

    public abstract void Jump();
    public abstract void UseSpecialAbility();

    public int CountPlayers()
    {
        Info = FindObjectOfType<LevelInfo>();
        int Count = 0;
        foreach (Player Obj in FindObjectsOfType<Player>())
            if (Obj.gameObject.activeInHierarchy && Info.CheckIfTheCharacterIsPlayable(Obj.Name))
                Count++;
        return Count;
    }

    protected void HeightCheck()
    {
        if (Rigidbody.velocity.y < -0.1)
        {
            Height += 1;
            Animator.SetBool("IsFalling", true);
        }
    }

    public void PlayerStartedDialogue()
    {
        IsActive = false;
        Rigidbody.isKinematic = true;
        Animator.SetBool("IsJumping", false);
        Animator.SetBool("IsFalling", false);
        Animator.SetFloat("Speed", 0);
    }

    public void PlayerEndedDialogue()
    {
        IsActive = true;
        Rigidbody.isKinematic = false;
    }
}
