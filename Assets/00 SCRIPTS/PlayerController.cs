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
    [SerializeField] PlayerState _playerState = PlayerState.IDLE;

    [SerializeField] AnimationControllerBase _animController;

    [SerializeField] List<PhysicsMaterial2D> physicMaterials = new List<PhysicsMaterial2D>();

    private void Awake()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _colli = GetComponent<Collider2D>();
    }

    void Update()
    {
        CheckPlatform();
        Moving();
        UpdateState();
        _animController.UpdateAnimation(_playerState);
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
                                                                            //Cách tối ưu:
        if (hasHit)
        {
            slopeTangent = new Vector2(hit.normal.y, -hit.normal.x);
            if (hit.collider.CompareTag("MovingPlatform"))
                transform.SetParent(hit.collider.transform);
            else
                this.transform.SetParent(null);
        }
        else
            this.transform.SetParent(null);

        //Cách tường minh:
        //if (hit.collider != null)
        //{
        //    _isOnGround = true;

        //    if (!(hit.normal.x == 0 && hit.normal.y == 1))
        //    {
        //        _isOnSlope = true;
        //        slopeTangent = new Vector2(hit.normal.y, -hit.normal.x);
        //    }
        //    else
        //        _isOnSlope = false;

        //    if (hit.collider.CompareTag("MovingPlatform"))
        //        transform.SetParent(hit.collider.transform);
        //    else
        //        transform.SetParent(null);
        //}
        //else
        //{
        //    _isOnGround = false;
        //    _isOnSlope = false;
        //    transform.SetParent(null);
        //}
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

    void UpdateState()
    {
        if (_playerState == PlayerState.HURT)
            return;

        if (!_isOnGround)
        {
            //_playerState = (_rigi.velocity.y > 0) ? PlayerState.JUMP : PlayerState.FALL;
            if (_rigi.velocity.y > 0)
            {
                _playerState = PlayerState.JUMP;
                _colli.sharedMaterial = physicMaterials[0];
            }
            else
                _playerState = PlayerState.FALL;
        }
        else
        {   
            if (Input.GetKey(KeyCode.W) && _rigi.velocity.x == 0)
            {
                _playerState = PlayerState.CLIMB;
            }
            else if (Input.GetKey(KeyCode.S))
                _playerState = PlayerState.DUCK;
            else if (Mathf.Abs(_rigi.velocity.x) >= 0.1f)
            {
                _playerState = PlayerState.RUN;
                _colli.sharedMaterial = physicMaterials[0];
            }
            else
            {
                _playerState = PlayerState.IDLE;
                _colli.sharedMaterial = physicMaterials[1];
            }
        }
    }

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        FALL,
        CLIMB,
        DUCK,
        HURT
    }
}
