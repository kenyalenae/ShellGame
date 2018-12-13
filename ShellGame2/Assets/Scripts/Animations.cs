using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Animations
{
    private Animator animator;

    private GameObject animatedObject;

    public Animations(GameObject animatedObject)
    {
        this.animatedObject = animatedObject;
    }

    public void AssignAnimators()
    {
        animator = animatedObject.GetComponent<Animator>();
    }

    public void AnimateItems()
    {
        animator.SetBool(Res.InMotion, true);
    }

    public void StopAnimations()
    {
        animator.SetBool(Res.InMotion, false);
    }

}

