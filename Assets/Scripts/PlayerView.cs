
using UI;
using UnityEngine;

using Zenject;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private float maxVelocity;
    [SerializeField] private int maxCapacity;
    [Range( 0,  100)]
    [SerializeField] private float speedDecreaseByStack; //percent to decrease speed when picking drop
    [SerializeField] private StacksView stacksView;
    
    //UI
    [SerializeField] private Joystick joystick;
    [Inject] private CapacityProgressBar capacityProgressBar;
    [Inject] private CoinsCounter coinsCounter;

    private Rigidbody rb;
    //anim
    private Animator animator;
    private int zoneCount = 0;
    private bool isWalking = false;

    //model
    [Header("Selling")]
    [SerializeField] private float capacityToMoneyK;

    private int coinsCount;
    //stacks
    private int capacity;
    private float stacksVisualInc;
    private float stacksCurVisualRate;
    //move
    private float curVel;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        curVel = maxVelocity;
        speedDecreaseByStack /= 100;
        stacksVisualInc= stacksView.MaxSize / (float)maxCapacity;
        coinsCount = PlayerPrefs.GetInt("Coins");
    }

   
    void FixedUpdate()
    {
        rb.velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * Time.deltaTime * curVel;
        
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
            animator.speed = curVel / (float) maxVelocity;
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
        if (drop!=null)//pick a drop
        {
            if (capacity==maxCapacity)
            {
                return;
            }
            
            ChangeCapacity(drop.Value);
            DecreaseSpeed();
            capacityProgressBar.ChangeValue((float)capacity/(float)maxCapacity);


            var nextVisRate = stacksCurVisualRate + stacksVisualInc;
            if (Mathf.Floor(nextVisRate)>Mathf.Floor(stacksCurVisualRate) 
                || capacity==maxCapacity)//can show next stack
            {
                stacksView.AddToStack();
            }
            stacksCurVisualRate += stacksVisualInc;
            
            Destroy(drop.gameObject);

            return;
        }
        
        var sell = other.gameObject.GetComponentInParent<SellZone>();
        if (sell != null)
        {
          
            PlayerPrefs.SetInt("Coins",coinsCount);
            coinsCount += (int) (capacity * capacityToMoneyK); //new coins count
            coinsCounter.SetVisualCoinValue((capacity * capacityToMoneyK)/stacksView.CurSize);  //value of one visual coin/stack
            
            stacksView.MoveStacksToTarget(sell.WheatReceiver.transform);
            coinsCounter.EnableReceiver();
            
            capacity = 0;
            capacityProgressBar.ChangeValue(0f);
            stacksCurVisualRate = 0;
            curVel = maxVelocity;

            return;
        }
    }

    private void DecreaseSpeed()
    {
        curVel -= curVel * speedDecreaseByStack;
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