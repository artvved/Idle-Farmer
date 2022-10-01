
using UI;
using UnityEngine;

using Zenject;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private int maxCapacity;
    [SerializeField] private StacksView stacksView;
    
    //UI
    [SerializeField] private Joystick joystick;
    [Inject] private CapacityProgressBar capacityProgressBar;

    private Rigidbody rb;
    //anim
    private Animator animator;
    private int zoneCount = 0;
    private bool isWalking = false;

    //model
    private int capacity;
    private float slowVel;
    private float stacksVisualInc;
    private float stacksCurVisualRate;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        slowVel = velocity *3/4;

        stacksVisualInc= stacksView.Limit / (float)maxCapacity;
    }

   
    void FixedUpdate()
    {
        if (zoneCount>0)//harvesting
        {
            rb.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * slowVel;
        }else
            rb.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * velocity;
        
        if (joystick.Direction.x != 0 || joystick.Direction.y != 0)
        {
            isWalking = true;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            AnimateWalk();
        }
        else
        {
            isWalking = false;
            rb.angularVelocity = Vector3.zero;
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
            if (capacity==maxCapacity)
            {
                return;
            }
            //pick a drop
            ChangeCapacity(drop.Value);
            capacityProgressBar.ChangeValue((float)capacity/(float)maxCapacity);

            var nextVisRate = stacksCurVisualRate + stacksVisualInc;
            if (Mathf.Floor(nextVisRate)>Mathf.Floor(stacksCurVisualRate))
            {
                stacksView.AddToStack();
            }
            Destroy(drop.gameObject);
            

            stacksCurVisualRate += stacksVisualInc;


            return;
        }
        
        var sell = other.gameObject.GetComponentInParent<SellZone>();
        if (sell != null)
        {
            capacity = 0;
            capacityProgressBar.ChangeValue(0f);
            stacksCurVisualRate = 0;

            return;
        }
    }

    private void ChangeCapacity(int val)
    {
        if (capacity+val>maxCapacity)
        {
            capacity = maxCapacity;
        }
        else
        {
            capacity += val;
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