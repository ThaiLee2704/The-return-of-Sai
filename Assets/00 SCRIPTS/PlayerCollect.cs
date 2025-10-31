using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private PlayerHealth _health;
    private PlayerHUDController _hud;
    private int _starCount = 0;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
    }
    private void Start()
    {
        _hud = GameManager.Instant.PlayerHUDController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            _starCount++;
            _hud.UpdateStarCount(_starCount);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Carrot"))
        {
            _health.Heal(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Trap"))
        {
            _health.TakeDamage(3);
        }
    }
}
