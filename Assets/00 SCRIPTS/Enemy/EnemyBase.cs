using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour //Tạo là 1 abs class để không thể instance trực tiếp từ class này
{
    [Header("Enemy Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int damage = 1;

    [Header("Components")]
    protected Rigidbody2D _rb;
    //protected Animator _anim;
    protected Collider2D _collider;
    protected Transform _player;

    protected bool _isDead = false;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_anim = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (_isDead) return;
        Move();
    }

    // ----------- Các hành vi chung -----------
    protected virtual void Move() { }   //Nên để là virtual vì ở class enemy con có những loại enemy
                                        //không di chuyển gì, nên nếu để là abs func thì các class con
                                        //sẽ bắt buộc phải override hàm di chuyển đó, vậy nên chúng ta
                                        //k nên để abs func ở đây
    protected virtual void Attack() { }

    public virtual void TakeDamage()
    {
        if (_isDead) return;

        _isDead = true;
        //_anim.SetTrigger("Die");
        _rb.velocity = Vector2.zero;
        _collider.enabled = false;

        // Hủy object sau 1 giây
        Destroy(transform.parent.gameObject, 1f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) return;

        if (collision.collider.CompareTag(CONSTANT.PLAYER_TAG))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(CONSTANT.PLAYER_TAG))
        {
            TakeDamage();
        }
    }

    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
