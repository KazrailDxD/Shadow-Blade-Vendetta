using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] public float jumpForce = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded = true;

    public SpriteRenderer flip { get; private set ; }
    Rigidbody2D Player;
    Vector3 moveDirection = new Vector3();

    IEnumerator dashCoroutine;
    bool isDashing;
    bool canDash = true;

    float dashDirection = 1;

    //Animationen
    Animator animator = null;

    //Sound Player
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip dashClip;

    public ParticleSystem dust;
    public Transform dancingDust;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
        flip = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
       
    }

    void Update()
    {
   
        if (moveDirection.x < 0)
        {
            flip.flipX = true;
            dashDirection = -1;
            dancingDust.transform.eulerAngles = new Vector2(0, -180);
        }
        else if (moveDirection.x > 0)
        {
            flip.flipX = false;
            dashDirection = 1;
            dancingDust.transform.eulerAngles = new Vector2(0,0);
        }

        if (isGrounded && moveDirection.x > 0 || moveDirection.x<0) 
        {
            CreateDust();
        }
       
        
        moveDirection.x = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        //Animation laufen
        animator.SetFloat("Run", Mathf.Abs(moveDirection.x));


        Jump();


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash==true)
        { 

            if (dashCoroutine != null) 
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(.25f, .5f);
            StartCoroutine(dashCoroutine);
            playerAudio.PlayOneShot(dashClip);
        }

    }

    private void FixedUpdate()
    {
        if (isDashing) 
        {
            CreateDust();
            Player.AddForce(new Vector2(dashDirection * 1.5f,0), ForceMode2D.Impulse);
        }
    }

    void Jump()
    {
        isGrounded = IsOnGround();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            CreateDust();
            animator.SetTrigger("BushJumping");
            Player.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            playerAudio.PlayOneShot(jumpClip);
        }

        if(isGrounded == true) 
        {
            animator.SetBool("IsJumping",false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }
       

    }
    
    IEnumerator Dash(float dashDuration, float dashCooldown)
    {
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        Player.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void AddJumpForce(float _amount) 
    {
        StartCoroutine(AddJumpForceCo(_amount));
    }
    private IEnumerator AddJumpForceCo(float _jumphight)
    {

        jumpForce += _jumphight;
        yield return new WaitForSeconds(3);
        jumpForce -= _jumphight;
    }

    public void CreateDust() 
    {
        dust.Play();
    }

    public bool IsOnGround() 
    {
        float height = 0.1f;
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, height, groundLayer);
        Color rayColor;
        if (ray.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + height), rayColor);
        return ray.collider != null;
    }

}
