using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private bool IsInRange;

    private ControlesPlayer playerMovement;

    [SerializeField]
    private BoxCollider2D ladder_top_collider;

    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlesPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = false;
            ladder_top_collider.isTrigger = false;
            return;
        }

        if (IsInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = true;
            ladder_top_collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInRange = false;
            playerMovement.isClimbing = false;
            ladder_top_collider.isTrigger = false;
        }
    }

}
