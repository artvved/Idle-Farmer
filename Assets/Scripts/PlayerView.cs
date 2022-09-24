using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;
    [SerializeField] private float velocity;
    private float slowVel;
    [SerializeField] private Joystick joystick;

    private int zoneCount = 0;
    private bool isWalking = false;
  

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        slowVel = velocity /2 ;
    }

   
    void FixedUpdate()
    {
        if (zoneCount>0)
        {
            rigidbody.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * slowVel;
        }else
            rigidbody.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * velocity;
        
        if (joystick.Direction.x != 0 || joystick.Direction.y != 0)
        {
            isWalking = true;
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
            //animator.SetBool("Walk",true);
            AnimateWalk();
        }
        else
        {
            isWalking = false;
            //animator.SetBool("Walk",false);
            rigidbody.angularVelocity = Vector3.zero;
            AnimateWalk();
        }
        
    }

    private void AnimateWalk()
    {
        if (!isWalking)
        {
            animator.SetBool("WalkAttack",false);
            animator.SetBool("Walk",false);
            return;
        }
        
        if (zoneCount>0)
        {
            animator.SetBool("WalkAttack",true);
            animator.SetBool("Walk",false);
        }
        else
        {
            animator.SetBool("Walk",true);
            animator.SetBool("WalkAttack",false);
        }
        
    }

    private void AnimateAttack()
    {
        if (zoneCount>0)
        {
            animator.SetBool("Attack",true);
        }
        else
        {
            animator.SetBool("Attack",false);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
      
       
        var pl = other.gameObject.GetComponent<WheatChunk>();
        if (pl!=null)
        {
            zoneCount++;
            AnimateAttack();
            AnimateWalk();
            return;
        }
        var drop = other.gameObject.GetComponentInParent<WheatDropView>();
        if (drop!=null)
        {
            //pick a drop
            print("pick");
            Destroy(drop.gameObject);
        }
        
        
    }
    

    private void OnTriggerExit(Collider other)
    {
        var pl = other.gameObject.GetComponent<WheatChunk>();
        if (pl!=null)
        {
            if (--zoneCount==0)
            {
                AnimateAttack();
                AnimateWalk();
            }
           
        }
    }
}