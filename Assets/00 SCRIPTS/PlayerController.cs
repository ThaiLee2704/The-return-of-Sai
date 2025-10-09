using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rigi;
    Collider2D _colli;
    Vector2 slopeTangent;

    [SerializeField] float _speed = 0, _jumpForce = 0;
    [SerializeField] bool _isOnGround = false, _isOnSlope = false;

    private void Awake()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _colli = GetComponent<Collider2D>();
    }

    void Update()
    {
        CheckPlatform();
        Moving();
    }

    void CheckPlatform()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (_colli.bounds.size.y / 2));

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.2f);
        Debug.DrawRay(rayOrigin, Vector2.down * 0.2f, Color.red);
        Debug.DrawRay(hit.point, hit.normal, Color.green);

        bool hasHit = hit.collider != null; //Khi có va chạm thì hasHit = true
        _isOnGround = hasHit;   //Khi có va chạm đồng nghĩa với chạm đất
        _isOnSlope = hasHit && !(hit.normal.x == 0 && hit.normal.y == 1);   //Khi ở trên dốc đồng nghĩa với việc ở
                                                                            //trên đất và vector pháp tuyển khác (0,1)
        if (hasHit)
        {
            slopeTangent = new Vector2(hit.normal.y, -hit.normal.x);
            if (hit.collider.CompareTag("MovingPlatform"))
                transform.SetParent(hasHit ? hit.collider.transform : null);
        }
        else
            this.transform.SetParent(null);
    }

    void Moving()
    {
        #region Move
        Vector2 movement = _rigi.velocity;

        if (!_isOnSlope)
        {
            movement.x = _speed * Input.GetAxisRaw("Horizontal");
        }
        else if (_isOnGround)
        {
            movement = slopeTangent.normalized * Input.GetAxisRaw("Horizontal") * _speed;
        }

        _rigi.velocity = movement;
        #endregion

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _isOnGround = false;
            _rigi.AddForce(new Vector2(0, _jumpForce));
        }
        #endregion

        #region Swap Scale
        if (_rigi.velocity.x > 0)
        {
            Vector2 scale = this.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            this.transform.localScale = scale;
        }

        if (_rigi.velocity.x < 0)
        {
            Vector2 scale = this.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            this.transform.localScale = scale;
        }
        #endregion
    }
}
