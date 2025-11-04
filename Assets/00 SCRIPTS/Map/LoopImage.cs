using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopImage : MonoBehaviour
{
    Texture _texture;
    [SerializeField] int _pixelPerUnit;
    [SerializeField] float _inGameWidth;

    [SerializeField] Transform _playerTransform;

    private void Awake()
    {
        _texture = this.GetComponent<SpriteRenderer>().sprite.texture;

        _inGameWidth = _texture.width/(float)_pixelPerUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameManager.Instant.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_playerTransform.position.x - this.transform.position.x) >= _inGameWidth)
        {
            Vector3 pos = this.transform.position;
            pos.x = _playerTransform.position.x;
            this.transform.position = pos;
        }
    }
}
