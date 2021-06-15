using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    private GameObject objButton;
    private Animator animButton;
    public void Start()
    {
        objButton = gameObject;
        animButton = objButton.GetComponent<Animator>();
    }
    public void OnClickAnimation()
    {
        animButton.SetTrigger("PressAnim");
    }
    public virtual void OnAnimationEnd()
    {
        Debug.Log("Animation End reached");
    }
}
