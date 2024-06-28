using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimatorController : MonoBehaviour
{
    public Animator anim;
    private void Start()
    {
        anim.SetTrigger("Cake");
    }
}
