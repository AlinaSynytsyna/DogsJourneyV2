using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected SaveLoadSystem SaveLoadSystem;
    protected string Name;
    //protected NodeVisitedTracker NodeTracker;
    //protected NPC Self;
    protected LevelInfo Info;
    //public NPC TargetNPC = null;
    //public DialogueRunner Runner;
    public bool IsActive = false;
    public CharacterChanger CharacterChanger;
    public PlayerCamera PlayerCamera;
    public CustomInput _customInput;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public int Health = 100;
    [SerializeField]
    protected float JumpForce;
    protected Rigidbody2D Rigid;
    protected Animator Animator;
    protected SpriteRenderer Renderer;
    public bool OnGround;
    public LayerMask GroundLayer;
    public bool IsWalking;
    protected float Timer = 0;
    protected bool IsJumping = false;
    protected int IdleState = 0;
    public int Height = 0;
    public float InteractionRadius;
    public void Start()
    {
        _customInput = CustomInputManager.GetCustomInputKeys();
    }

    public abstract bool GroundCheck();

    protected virtual void Awake()
    {
        this.enabled = true;
        Rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public abstract void Walk();

    public abstract void Jump();
    public abstract void SpecialAbility();
    public abstract void CheckCharacter();

    public int CountPlayers()
    {
        Info = FindObjectOfType<LevelInfo>();
        int Count = 0;
        foreach (Player Obj in FindObjectsOfType<Player>())
            if (Obj.gameObject.activeInHierarchy && Info.CheckCharacter(Obj.Name))
                Count++;
        return Count;
    }

    protected void HeightCheck()
    {
        if (Rigid.velocity.y < -0.1)
        {
            Height += 1;
            Animator.SetBool("IsFalling", true);
        }
    }
}
