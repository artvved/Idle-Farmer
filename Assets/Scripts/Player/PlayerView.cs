using Player;
using UI;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private float maxVelocity;
    [SerializeField] private int maxCapacity;
    [Range(0, 100)] [SerializeField] private float speedDecreaseByStack; //percent to decrease speed when picking drop
    [SerializeField] private StacksView stacksView;

    //UI
    [SerializeField] private MenuScreen menuScreen;
    [Inject] private CapacityProgressBar capacityProgressBar;
    [Inject] private CoinsCounter coinsCounter;

    private Rigidbody rb;

    //input
    private IInputManager inputManager;

    //anim
    private PlayerAnimationView playerAnimationView;

    //model
    [Header("Selling")] [SerializeField] private float capacityToMoneyK;

    private int coinsCount;

    //stacks
    private int capacity;
    private float stacksVisualInc;

    private float stacksCurVisualRate;

    //move
    private float curVel;

    void Start()
    {
        playerAnimationView = new PlayerAnimationView(GetComponent<Animator>());
        rb = GetComponent<Rigidbody>();
        curVel = maxVelocity;
        speedDecreaseByStack /= 100;
        stacksVisualInc = stacksView.MaxSize / (float) maxCapacity;
        coinsCount = PlayerPrefs.GetInt("Coins");

        InitInputManagers();
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Input")))
        {
            PlayerPrefs.SetString("Input", "Joystick");
        }
        menuScreen.ToggleSwitched += EnableInputManager;
        EnableInputManager();
    }

    private void InitInputManagers()
    {
        foreach (var manager in GetComponents<IInputManager>())
        {
            if (manager.GetType() == typeof(JoystickInputManager))
            {
                manager.InputEvent += JoystickMove;
                continue;
            }

            if (manager.GetType() == typeof(SwipeInputManager))
            {
                manager.InputEvent += SwipeMove;
                continue;
            }
            
            if (manager.GetType() == typeof(GyroscopeInputManager))
            {
                manager.InputEvent += GyroscopeMove;
                continue;
            }
        }
    }

    private void EnableInputManager()
    {
        foreach (var manager in GetComponents<IInputManager>())
        {
            manager.enabled = false;
        }

        switch (PlayerPrefs.GetString("Input"))
        {
            case "Joystick":
                inputManager = GetComponent<JoystickInputManager>();
                break;
            case "Swipe":
                inputManager = GetComponent<SwipeInputManager>();
                break;
            case "Gyroscope":
                inputManager = GetComponent<GyroscopeInputManager>();
                break;
        }

        inputManager.enabled = true;
    }


    private void JoystickMove(Vector2 direction)
    {
        rb.velocity = new Vector3(direction.x, 0, direction.y) * curVel;

        if (direction.x != 0 || direction.y != 0)
        {
            playerAnimationView.IsWalking = true;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
        else
        {
            playerAnimationView.IsWalking = false;
            rb.angularVelocity = Vector3.zero;
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
    }

    private void SwipeMove(Vector2 direction)
    {
        rb.velocity = new Vector3(direction.x, 0, direction.y) * curVel;

        if (direction.x != 0 || direction.y != 0)
        {
            playerAnimationView.IsWalking = true;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
        else
        {
            playerAnimationView.IsWalking = false;
            rb.angularVelocity = Vector3.zero;
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
    }
    
    private void GyroscopeMove(Vector2 direction)
    {
        if (direction.magnitude<0.2f)
        {
            return;
        }
        rb.velocity = new Vector3(direction.x, 0, direction.y) * curVel;

        if (direction.x != 0 || direction.y != 0)
        {
            playerAnimationView.IsWalking = true;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
        else
        {
            playerAnimationView.IsWalking = false;
            rb.angularVelocity = Vector3.zero;
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        var pl = other.gameObject.GetComponent<WheatChunk>();
        if (pl != null)
        {
            playerAnimationView.IncZoneCount();
            ;
            playerAnimationView.AnimateAttack();
            playerAnimationView.AnimateWalk(curVel, maxVelocity);
            return;
        }

        var drop = other.gameObject.GetComponentInParent<WheatDropView>();
        if (drop != null) //pick a drop
        {
            if (capacity == maxCapacity)
            {
                return;
            }

            ChangeCapacity(drop.Value);
            DecreaseSpeed();
            capacityProgressBar.ChangeValue((float) capacity / (float) maxCapacity);


            var nextVisRate = stacksCurVisualRate + stacksVisualInc;
            if (Mathf.Floor(nextVisRate) > Mathf.Floor(stacksCurVisualRate)
                || capacity == maxCapacity) //can show next stack
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
            PlayerPrefs.SetInt("Coins", coinsCount);
            coinsCount += (int) (capacity * capacityToMoneyK); //new coins count
            coinsCounter.SetVisualCoinValue((capacity * capacityToMoneyK) /
                                            stacksView.CurSize); //value of one visual coin/stack

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
        if (capacity + val > maxCapacity)
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
        if (pl != null)
        {
            playerAnimationView.DecZoneCount();
            if (playerAnimationView.GetZoneCount() == 0)
            {
                playerAnimationView.AnimateAttack();
                playerAnimationView.AnimateWalk(curVel, maxVelocity);
            }
        }
    }
}