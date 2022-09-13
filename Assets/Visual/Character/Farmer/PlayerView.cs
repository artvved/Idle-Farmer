using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;
    [SerializeField] private float velocity;
    [SerializeField] private Joystick joystick;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        rigidbody.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * velocity;
        if (joystick.Direction.x != 0 || joystick.Direction.y != 0)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
            animator.SetBool("Walk",true);
        }
        else
        {
            animator.SetBool("Walk",false);
            rigidbody.angularVelocity = Vector3.zero;
        }


        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Walk");
            rigidbody.velocity = Time.deltaTime * velocity * Vector3.forward;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("Walk");
            rigidbody.velocity = Time.deltaTime * velocity * Vector3.back;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetTrigger("Idle");
            rigidbody.velocity = Vector3.zero;
        }*/
    }
}