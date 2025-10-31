using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField]
    List<LoopImage> _loopingBGs = new List<LoopImage>();    //List các BG

    [SerializeField] float _baseSpeed, _configSpeed;

    [SerializeField] float _way;    //Hướng của BG lúc player chạy sẽ ngược với hướng chạy của player

    [SerializeField] float _oldPosX;    //Thêm 1 biến tọa độ cũ, nhằm mục đích nếu player chạy mắc cái gì đó thì Parallax không xảy ra
    
    // Update is called once per frame
    void Update()
    {
        _way = GameManager.Instant.Player.transform.localScale.x < 0 ? 1 : -1;  //Player chạy sang trái thì BG sẽ chạy sang phải để tạo cảm giác ngược chiều, ngược lại

        if (GameManager.Instant.Player.playerState == PlayerController.PlayerState.IDLE)
            return;
        //if (GameManager.Instant.Player.playerState == PlayerController.PlayerState.JUMP)
        //    return;
        if (GameManager.Instant.Player.transform.position.x == _oldPosX)    //Khi mà tọa độ player bằng giữ nguyên thì Paralax không xảy ra
            return;

        for (int i = 0; i < _loopingBGs.Count; i++)
        {
            //Cách dùng List:
            //_loopingBGs[i].transform.position += new Vector3(_way * _speedsBG[i] * Time.deltaTime, 0, 0); //Vị trí pos.x của BG sẽ + thêm 1 đoạn bằng hướng * tốc độ của BG đó

            //Cách khác mà không cần dùng List các tốc độ của BG
            float speed = i * _baseSpeed * _configSpeed;    //Tốc độ sẽ tăng theo thứ tự BG, i càng cao thì càng nhanh
                                                            //Nó sẽ bằng i * _baseSpeed * _configSpeed, mục đích của
                                                            //_configSpeed nhằm điều chỉnh nếu tốc độ giữa 2 BG chênh quá nhiều
            _loopingBGs[i].transform.position += new Vector3(_way * speed * Time.deltaTime, 0, 0);
        }
    }

    private void LateUpdate()
    {
        _oldPosX = GameManager.Instant.Player.transform.position.x; //Lấy tọa độ cuối cùng của Update(), ở đây là Update của Player
    }
}
