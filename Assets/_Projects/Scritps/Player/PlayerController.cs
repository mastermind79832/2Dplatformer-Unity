using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static private PlayerController s_PlayerInstance;
    static public PlayerController playerInstance { get {return s_PlayerInstance;} }

    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 respawnPoint;
    public UIManager uI;
    private bool isGrounded, isCrouch, isSecondJump;

    private bool[] keys;
    private float timeOut;

    void Awake()
    {
        s_PlayerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
        isGrounded = false;
        isCrouch = false;
        isSecondJump = false;
        keys = new bool[3];
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        IncreaseTimeoutTime();
    }

    private void IncreaseTimeoutTime()
    {
        timeOut += Time.deltaTime;
    }

    public Vector3 GetLocation() {  return transform.position;  }

    /** Vertical Movement
     * Jump
     * Crouch
    */
    private void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Vector2 velocity = rb.velocity;

        //  Horizintal Movement  
        if(xInput != 0 && !isCrouch) 
        {
            Vector2 scale = transform.localScale;
            int side = ((xInput > 0)?1:-1);
            scale.x = Mathf.Abs(scale.x) * side;
            velocity.x = movementSpeed * side;
            transform.localScale = scale;
        }
        else
            velocity.x = 0;

        //  Jump and Crouch
        if(Input.GetKeyDown(KeyCode.Space) && (isGrounded || isSecondJump))
        {
            anim.SetTrigger("Jump");
            // if(isSecondJump)
            //     velocity.y += jumpSpeed * 1.1f; // TO DO
            // else    
                velocity.y = jumpSpeed;
            isSecondJump = !isSecondJump;
        }
        else if(yInput < 0)
            isCrouch = true;
        else   
            isCrouch = false;

        anim.SetFloat("VerticalVelocity",rb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));  
        anim.SetBool("IsCrouch",isCrouch);
        rb.velocity = velocity;
    }

     private void AddKeysToInventory()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if(!keys[i])
            {
                keys[i] = true;
                uI.KeysObtained(i);
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Key")
        {
            AddKeysToInventory();
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Acid") && timeOut > 2)
            respawn();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy") )
            respawn();
    }

    private void respawn()
    {
        timeOut = 0;
        uI.HealthLost();
        if(uI.GetRemainingHealth() <= 0 )
        {
            StartCoroutine(WaitForDeath()); 
            return;
        }
        StartCoroutine(Hurt());
    }

    IEnumerator Hurt()
    {
        anim.Play("Hurt");
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WaitForDeath()
    {
        anim.SetTrigger("IsDead");
        yield return new WaitForSeconds(0.5f);
        transform.position = respawnPoint;
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        //GAmeOverSCene
        uI.SetGameOverPanel();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(isColliderGround(other.collider, "Ground"))
        {
            transform.parent = null;
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
            isSecondJump = false;
        }

        if(isColliderGround(other.collider,"Platform"))
        {
            transform.parent = other.gameObject.transform;
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
            isSecondJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(isNotCollidingGround(other.collider, "Ground"))
        {
            isGrounded = false;
            anim.SetBool("IsGrounded", false);
        }

        if(isNotCollidingGround(other.collider, "Platform"))
        {
            transform.parent = null;
            isGrounded = false;
            anim.SetBool("IsGrounded", false);
        }
    }

    private bool isColliderGround(Collider2D other, string tag)
    {
        return other.gameObject.CompareTag(tag) && groundCheck.IsTouching(other);
    }

    private bool isNotCollidingGround(Collider2D other, string tag)
    {
        return other.gameObject.CompareTag(tag) && !groundCheck.IsTouching(other);
    }
}


