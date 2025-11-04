using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnail : EnemyBase
{
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private Transform _targetPoint;

    protected override void Start()
    {
        base.Start();
        _targetPoint = pointB;
    }

    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, moveSpeed * Time.deltaTime);

        // Đến điểm thì đổi hướng
        if (Vector2.Distance(transform.position, _targetPoint.position) < 0.1f)
        {
            _targetPoint = _targetPoint == pointA ? pointB : pointA;
            Flip();
        }
    }

    
}

