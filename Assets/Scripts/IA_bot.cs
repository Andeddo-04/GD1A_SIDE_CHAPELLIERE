using UnityEngine;

public class IA_bot : MonoBehaviour
{
    [SerializeField]
    private bool isDead = false;

    public int damageOnCollision = 10;

    [SerializeField]
    private Rigidbody2D mobSprite;

    [SerializeField]
    private SpriteRenderer mobSpriteRenderer;

    [SerializeField]
    private float speed, jumpForce;

    public Transform[] waypoints;

    private Transform target;
    private int desPoint;

    [SerializeField]
    private bool isGrounded, isTouchingLeftWall, isTouchingRightWall;

    [SerializeField]
    private Transform groundCheck, leftWallCheck, rightWallCheck;

    [SerializeField]
    private float groundCheckRadius, leftWallCheckRadius, rightWallCheckRadius;

    [SerializeField]
    private LayerMask collisionLayer;






    void Start()
    {
        target = waypoints[0];
    }


    void Update()
    {
        if (!isDead)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);

            isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, leftWallCheckRadius, collisionLayer);

            isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, rightWallCheckRadius, collisionLayer);


            Vector3 direction = target.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);


            // Si l'ennemie est quasiment arrivé a sa destination
            if (Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                desPoint = (desPoint + 1) % waypoints.Length; // % = reste division
                target = waypoints[desPoint];
                mobSpriteRenderer.flipX = !mobSpriteRenderer.flipX;
            }

            // Permet au monstre de sauter s'il va percuter un relief du sol
            if ((isTouchingLeftWall && isGrounded) || (isTouchingRightWall && isGrounded))
            {
                mobSprite.AddForce(new Vector2(0f, jumpForce));

            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerHealth playerHealth = collision.transform.GetComponent<playerHealth>();
            playerHealth.TakeDamage(10);
        }
    }
    


}