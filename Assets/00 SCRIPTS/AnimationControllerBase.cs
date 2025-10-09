using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerBase : MonoBehaviour
{
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0) 
            _anim.SetBool("Run", true);
        else
            _anim.SetBool("Run", false);
    }
}
