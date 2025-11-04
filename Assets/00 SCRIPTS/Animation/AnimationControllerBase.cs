using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = PlayerController.PlayerState;

public class AnimationControllerBase : MonoBehaviour
{
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(PlayerState playerState)
    {
        for (int i = 0; i <= (int)PlayerState.HURT; i++)
        {
            string stateName = ((PlayerState)i).ToString();
            if (playerState == (PlayerState)i)
                _animator.SetBool(stateName, true);
            else
                _animator.SetBool(stateName, false);
        }
    }
}
