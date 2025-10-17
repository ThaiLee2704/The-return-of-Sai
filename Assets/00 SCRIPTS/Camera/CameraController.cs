using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Vector2 _camOffset;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instant.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _player.position + (Vector3)_camOffset;
        pos.z = Camera.main.transform.position.z;

        Camera.main.transform.position = pos;
    }
}
