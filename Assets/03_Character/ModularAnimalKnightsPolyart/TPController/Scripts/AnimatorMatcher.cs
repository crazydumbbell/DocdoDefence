﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{

    public class AnimatorMatcher : MonoBehaviour
    {
        Animator anim;
        ControlManager control;
        
        void Start()
        {
            anim = GetComponent<Animator>();
            control = GetComponentInParent<ControlManager>();
        }

        private void OnAnimatorMove()   //It updates every frame when animator's animations in play.
        {
            if (control.canMove)
                return;

            if (!control.onGround)
                return;

            control.rigid.linearDamping = 0;
            float multiplier = 3f;

            Vector3 dPosition = anim.deltaPosition;   //storing delta positin of active model's position.         
            
            dPosition.y = 0f;   //flatten the Y (height) value of root animations.
            
            Vector3 vPosition = (dPosition * multiplier) / Time.fixedDeltaTime;     //defines the vector 3 value for the velocity.      
            
            control.rigid.linearVelocity = vPosition; //This will move the root gameObject for matching active model's position.
        
        }
    }
}