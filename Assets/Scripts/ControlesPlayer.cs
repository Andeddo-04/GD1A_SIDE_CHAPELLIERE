using UnityEngine;

public class ControlesPlayer : MonoBehaviour
{
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    private Rigidbody2D characterSprite;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private bool isGrounded, isTouchingLeftWall, isTouchingRightWall, isJumping = false;
    
    public bool isClimbing;

    [SerializeField]
    private float moveSpeed, jumpForce, wallJumpForce;

    [SerializeField]
    private Transform groundCheck, leftWallCheck, rightWallCheck;

    [SerializeField]
    private float groundCheckRadius, wallCheckRadius;

    [SerializeField]
    private LayerMask groundCollisionLayer, wallCollisionLayer;

    private bool hasJumpedFromLeftWall = false;
    private bool hasJumpedFromRightWall = false;

    private Vector2 lastMoveDirection = Vector2.right; // Store last move direction

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCollisionLayer);
        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, wallCollisionLayer);
        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, wallCollisionLayer);

        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement, verticalMovement);

        // Flip the character based on last move direction
        if (horizontalMovement != 0)
        {
            lastMoveDirection = new Vector2(horizontalMovement, 0f);
            Flip();
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isClimbing", isClimbing);

        if (Input.GetButtonDown("Jump"))
        {
            if ((isTouchingLeftWall && !hasJumpedFromLeftWall) && !isClimbing)
            {
                WallJump(Vector2.right);
                hasJumpedFromLeftWall = true;
                hasJumpedFromRightWall = false; // Reset wall jump opposite direction
            }
            else if ((isTouchingRightWall && !hasJumpedFromRightWall) && !isClimbing)
            {
                WallJump(Vector2.left);
                hasJumpedFromRightWall = true;
                hasJumpedFromLeftWall = false; // Reset wall jump opposite direction
            }
            else if (isGrounded && !isClimbing)
            {
                characterSprite.velocity = new Vector2(characterSprite.velocity.x, jumpForce);
                hasJumpedFromLeftWall = false;
                hasJumpedFromRightWall = false;
                
            }
            
        }
        
    }

    void WallJump(Vector2 direction)
    {
        characterSprite.velocity = new Vector2(wallJumpForce * direction.x, jumpForce);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, characterSprite.velocity.y);
            characterSprite.velocity = Vector3.SmoothDamp(characterSprite.velocity, targetVelocity, ref velocity, 0.05f);
        }

        else
        {
            // déplacement verticale
            Vector3 targetVelocity = new Vector2(0, _verticalMovement);
            characterSprite.velocity = Vector3.SmoothDamp(characterSprite.velocity, targetVelocity, ref velocity, 0.05f);
        }
    }

    void Flip()
    {
        if (lastMoveDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (lastMoveDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}
