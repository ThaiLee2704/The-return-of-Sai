using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
    private Rigidbody2D _rigi;
    private float horizontal;
    [SerializeField] private float vertical;

    [SerializeField] private bool isOnLadder = false;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private float originalGravity;

    void Start()
    {
        _rigi = GetComponent<Rigidbody2D>();
        originalGravity = _rigi.gravityScale;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Nếu đang trong vùng thang
        if (isOnLadder)
        {
            if (Mathf.Abs(vertical) > 0.1f)
            {
                isClimbing = true;
            }
        }

        // Khi leo
        if (isClimbing)
        {
            _rigi.gravityScale = 0f;
            _rigi.velocity = new Vector2(horizontal * moveSpeed, vertical * climbSpeed);
        }
        else
        {
            _rigi.gravityScale = originalGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(CONSTANT.LADDER_TAG))
        {
            isOnLadder = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(CONSTANT.LADDER_TAG))
        {
            isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(CONSTANT.LADDER_TAG))
        {
            isOnLadder = false;
            isClimbing = false;
        }
    }
}
