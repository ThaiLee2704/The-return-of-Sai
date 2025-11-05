using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] float moveSpeed = 2f;
    private Transform _targetPoint;

    private void Start()
    {
        _targetPoint = pointB;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _targetPoint.position) < 0.1f)
        {
            _targetPoint = _targetPoint == pointA ? pointB : pointA;
        }
    }
}
