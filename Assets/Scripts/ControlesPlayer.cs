using UnityEngine;

public class ControlesPlayer : MonoBehaviour
{
    // Initialisation des contrôles
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
    private bool isGrounded, isTouchingLeftWall, isTouchingRightWall;

    [SerializeField]
    private bool isJumping = false;

    [SerializeField]
    private float moveSpeed, jumpForce, wallJumpForceX, wallJumpForceY;

    [SerializeField]
    private Transform groundCheck, leftWallCheck, rightWallCheck;

    [SerializeField]
    private float groundCheckRadius, leftWallCheckRadius, rightWallCheckRadius;

    [SerializeField]
    private LayerMask groundCollisionLayer;

    [SerializeField]
    private LayerMask wallCollisionLayer;

    [SerializeField]
    private Transform wallCheck; // Ajout de la vérification pour le wall jump

    [SerializeField]
    private float wallCheckRadius; // Distance pour vérifier le mur

    [SerializeField]
    private bool isTouchingWall = false;

    // Méthode appelée à chaque frame
    void Update()
    {
        // Vérifier si le personnage est au sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCollisionLayer);

        // Vérifier si le personnage touche le mur
        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, leftWallCheckRadius, wallCollisionLayer);

        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, rightWallCheckRadius, wallCollisionLayer);

        if (isTouchingLeftWall || isTouchingRightWall)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }

        // Obtenir l'entrée horizontale (gauche/droite)
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement);

        Flip(characterSprite.velocity.x);

        float characterVelocity = Mathf.Abs(characterSprite.velocity.x);

        animator.SetFloat("Speed", characterVelocity);

        // Vérifier si la touche de saut est enfoncée et si le personnage est au sol ou touche le mur
        if (Input.GetKeyDown(jumpKey) && (isGrounded || isTouchingWall))
        {
            // Si le joueur touche le mur, effectuer le wall jump
            if (isTouchingWall && isTouchingLeftWall)
            {
                // Calculer la composante y de la vélocité pour obtenir une trajectoire courbée
                float jumpDirectionY = Mathf.Sqrt(2f * wallJumpForceY * Mathf.Abs(Physics2D.gravity.y)); 

                // Changer la direction du personnage en diagonale courbée vers le haut et vers la droite
                characterSprite.velocity = new Vector2(wallJumpForceX, jumpDirectionY);
            }
            else if (isTouchingWall && isTouchingRightWall)
            {
                // Calculer la composante y de la vélocité pour obtenir une trajectoire courbée
                float jumpDirectionY = Mathf.Sqrt(2f * wallJumpForceY * Mathf.Abs(Physics2D.gravity.y));

                // Changer la direction du personnage en diagonale courbée vers le haut et vers la gauche
                characterSprite.velocity = new Vector2(-wallJumpForceX, jumpDirectionY);
            }

            isJumping = true;
            animator.SetBool("isJumping", true);
        }
    }

    void MovePlayer(float _horizontalMovement) // _horizontalMovement => convention de nommage
    {
        // Définir la vitesse horizontale du personnage en fonction de l'entrée
        Vector3 targetVelocity = new Vector2(_horizontalMovement, characterSprite.velocity.y);
        characterSprite.velocity = Vector3.SmoothDamp(characterSprite.velocity, targetVelocity, ref velocity, 0.05f);

        if (isJumping == true)
        {
            // Appliquer une force vers le haut pour simuler le saut
            characterSprite.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (_velocity < 0.1f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
