using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigi;
    public Rigidbody2D Rigi => _rigi;
    private Collider2D _colli;
    private int _groundMask;
    private PlayerHealth _playerHealth;

    [Header("Physics")]
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _jumpForce = 0;
    [SerializeField] private float _rayLength = 1.4f;

    [Header("States")]
    [SerializeField] private bool _isOnGround = false;
    [SerializeField] private PlayerState _playerState = PlayerState.IDLE;
    public PlayerState playerState => _playerState;

    [Header("Animation")]
    [SerializeField] private AnimationControllerBase _animController;
    [SerializeField] float hurtTimer;

    [Header("Physics Materials")]
    [SerializeField] private List<PhysicsMaterial2D> _physicMaterials = new List<PhysicsMaterial2D>();

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        FALL,
        CLIMB,
        DUCK,
        DIE,
        HURT
    }

    private void Awake()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _colli = GetComponent<Collider2D>();
        _playerHealth = GetComponent<PlayerHealth>();
        _groundMask = LayerMask.GetMask(CONSTANT.GROUND_LAYER, CONSTANT.MOVING_PLATFORM_LAYER);
    }

    void Update()
    {
        if (_playerState != PlayerState.DIE)
            CheckGround();

        HandleMovement();
        HandleJump();
        HandleFlip();
        UpdateState();
        _animController.UpdateAnimation(_playerState);
    }

    #region --- Shooting Raycast to ground ---
    //Băn 1 tia ray giữa chân Player xuống dưới mặt đất, trả về true nếu chạm, false nếu null
    private bool RaycastToGround(out RaycastHit2D hit)
    {
        //Tạo gốc tọa độ của tia Ray
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (_colli.bounds.size.y / 2f));

        //Hướng bắn tia Ray
        Vector2 rayDirection = Vector2.down;

        //Thực hiện Raycast
        hit = Physics2D.Raycast(rayOrigin, rayDirection, _rayLength, _groundMask);

        //Debug Ray kiểm tra
        Debug.DrawRay(rayOrigin, rayDirection * _rayLength, hit ? Color.green : Color.red);

        return hit.collider != null;
    }
    #endregion

    #region --- Check Ground ---
    void CheckGround()
    {
        if (RaycastToGround(out RaycastHit2D hit))
        {
            if (Mathf.Abs(hit.normal.x) < 0.01f && hit.normal.y > 0.99f)    //~~ hit.normal ~= Vector2.up, so sánh với sai số
                _isOnGround = true;
            else
                _isOnGround = false;

            // Nếu đứng trên platform di chuyển
            if (hit.collider.CompareTag(CONSTANT.MOVING_PLATFORM_TAG))
                transform.SetParent(hit.collider.transform);
            else
                transform.SetParent(null);
        }
        else
        {
            _isOnGround = false;
            transform.SetParent(null);
        }
    }
    #endregion

    #region --- Movement ---
    private void HandleMovement()
    {
        Vector2 movement = _rigi.velocity;
        float inputX = Input.GetAxisRaw("Horizontal");

        movement.x = inputX * _speed;

        _rigi.velocity = new Vector2(movement.x, _rigi.velocity.y);
    }
    #endregion

    #region --- Jump ---
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _isOnGround = false;
            _rigi.velocity = new Vector2(_rigi.velocity.x, 0f); // reset vertical velocity
            _rigi.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            //ForceMode2D.Impulse tác động mạnh 1 lần tức thời chứ không theo thời gian
        }
    }
    #endregion

    #region --- Flip ---
    private void HandleFlip()
    {
        if (Mathf.Abs(_rigi.velocity.x) > 0.05f)    //Khi nhân vật đứng yên, chỉ khi di chuyển thật sự thì velocity.x mới > 0.05f (tầm đấy)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_rigi.velocity.x) * Mathf.Abs(scale.x);
            //Ta có Mathf.Sign(_rigi.velocity.x) sẽ trả về 1 hoặc -1 tùy theo giá trị _rigi.velocity.x >=0 hay <0
            //Nhưng nếu chỉ trả về 1 hoặc -1 thì đôi khi sai vì scale có thể là 1 số lớn hơn nên ta cần * Mathf.Abs(scale.x)
            //Dòng này có nghĩa là đặt lại hướng quay nhưng kích thước không thay đổi.
            transform.localScale = scale;
        }
    }
    #endregion

    #region --- State Update ---
    private void UpdateState()
    {
        if (_playerHealth.CurrentHealth <= 0 && _playerState != PlayerState.DIE)
        {
            _playerState = PlayerState.DIE;
            _rigi.velocity = Vector2.zero;
            _colli.enabled = false;   // Rơi xuyên sàn
            _animController.UpdateAnimation(PlayerState.HURT);
            StartCoroutine(EnableAfterDelay(this.gameObject, 3f));
            return;
        }

        if (_playerHealth.wasDamaged && _playerState != PlayerState.HURT)
        {
            _rigi.velocity = new Vector2(0, 0);
            _rigi.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
            _playerState = PlayerState.HURT;
            _playerHealth.wasDamaged = false;
            hurtTimer = 0;
            return;
        }

        if (_playerState == PlayerState.HURT)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer > 0.5f)
            {
                hurtTimer = 0;
                _playerState = PlayerState.IDLE;
            }
            return;
        }


        if (!_isOnGround)
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && _rigi.gravityScale == 0)   //WARNING: Đổi điều kiện _rigi.velocity.x == 0
            {
                _playerState = PlayerState.CLIMB;
            }
            else if (_rigi.velocity.y > 0 && _colli.enabled)
            {
                _playerState = PlayerState.JUMP;
                _colli.sharedMaterial = _physicMaterials[0];
                //Mục đích không có ma sát ở cạnh colli khi chạm cạnh của platform
            }
            else if (_rigi.velocity.y < 0 && _colli.enabled)
            {
                _playerState = PlayerState.FALL;
            }
            return;
        }

        if (Input.GetKey(KeyCode.J))
        {
            _playerState = PlayerState.DUCK;
        }
        else if (Mathf.Abs(_rigi.velocity.x) > 0.05f)
        {
            _playerState = PlayerState.RUN;
            _colli.sharedMaterial = _physicMaterials[0];
        }
        else
        {
            _playerState = PlayerState.IDLE;
            _colli.sharedMaterial = _physicMaterials[1];
        }

    }
    #endregion

    IEnumerator EnableAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(false);
    }
}
