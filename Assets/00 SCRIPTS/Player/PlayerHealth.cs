using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerHUDController _hud;

    [Header("Infomation")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    public bool wasDamaged = false;
    public event Action<int> OnHealthChanged; // UI lắng nghe để cập nhật
    public event Action OnPlayerDie;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        _hud = GameManager.Instant.PlayerHUDController;
        _hud.UpdateHealth(currentHealth);
        OnHealthChanged += _hud.UpdateHealth;   //Đăng kí sự kiện UpdateHealth cho OnHealthChanged
        //OnPlayerDie += cũng đky sự kiện Gameover chẳng hạn như cái UpdateHealth trên
    }

    public void TakeDamage(int amount = 1)  //Nếu gọi hàm mà không truyền tham số thì mặc định là 1
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        wasDamaged = true;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount = 1)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth); //gọi các sự kiện đã đky ở đây cụ thể là chỉ mỗi _hud.UpdateHealth()
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        OnPlayerDie?.Invoke();
        // Ở đây bạn có thể gọi hàm RestartLevel() hoặc bật UI Game Over
    }
}
