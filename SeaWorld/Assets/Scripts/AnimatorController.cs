using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class AnimatorController : MonoBehaviour
{
    [Space(5)]
    [Header("Logic behaivor")]
    [SerializeField] private bool disableSwimming = false;
    [SerializeField] private bool disableAttacking = false;
    [SerializeField] bool isInmortal = false;
    [Space(5)]
    [Header("Movement")]
    [SerializeField] private float swimmingSpeedMult = 0.15f;
    [SerializeField] private float forwardShiftMaxSpeed = 2;
    [SerializeField] private float animatorVelocityWithShift = 2.2f;
    [SerializeField] private float normalToShiftVelocityTime = 5;
    [SerializeField] bool autoPitchRollAlign = true;
    [SerializeField] float autoPitchRollSpeed = 0.1f;
    [Tooltip("The probability between 0-1 to change the idle animation")] [SerializeField] private float idleChangeProbability = 0.2f;
    [Tooltip("How many idle animation are inside IdleMachine")] [SerializeField] private int idleCountAnim = 4;

    [Space(5)]
    [Header("Movement with keyboard")]
    [Tooltip("All movement with keyboard, attacking with space. Use SimpleFixedCamera for better experience")]
    [SerializeField] private float yawSpeed = 1.2f;
    [SerializeField] private float rollSpeed = 1.2f;
    [SerializeField] private float pitchSpeed = 1.2f;
    [Range(1, 10)] [SerializeField] private float rotateShiftMultKeyboard = 4f;

    [Space(5)]
    [Header("Movement with mouse")]
    [SerializeField] private bool mouseRotationControlled = true;
    [SerializeField] private bool righMousePressedToRotate = true;
    //[SerializeField] private LayerMask lockAtRotationMask;
    //[SerializeField] private float yRayHitOffset = 4;
    [Range(1, 10)] [SerializeField] private float rotateShiftMultMouse = 4f;
    [SerializeField] private float rotateStopMultMouse = 4f;

    [Space(5)]
    [Header("Attacking and Hurt")]
    [SerializeField] private GameObject biteDetector;
    [SerializeField] private GameObject meatDetector;
    public GameObject biteParent;
    [SerializeField] int attackAnimation = 2;
    [SerializeField] bool allowParentToAttacker = true;
    [Tooltip("Between 0-1 probability to let go to the attacker mouth. Only are stick if the weight is lower than the attacker")]
    [SerializeField] private float chanceToLetGo = 0.8f;
    [Space(2)]
    [SerializeField] private bool isWhaleShark = false;
    [SerializeField] float timeAbsorbing = 2f;
    private float timeStartAbsorb = 0f;
    private BiteDectector attackerOb;
    private bool beenBiteFlag = false;
    private bool stickToBiteFlag = false;

    private Animator sharkAnim;
    //private AnimatorStateInfo sharkAnimStateInfo;
    private Rigidbody sharkRb;
    // Start is called before the first frame update


    #region AnimatorParameters
    private float animatorVelocity = 1;
    private bool isMoving = false;
    private bool isRolling = false;
    private float forward = 0;
    private float actualForward = 0;
    private float yaw = 0;
    private float pitch = 0;
    private float roll = 0;
    #endregion

    #region Animator_HashTag
    private static int doIdleHash = Animator.StringToHash("doIdle");
    private static int idleRandomHash = Animator.StringToHash("idleRandom");
    private static int isMovingHash = Animator.StringToHash("isMoving");
    private static int isRollingHash = Animator.StringToHash("isRolling");
    private static int isAttackingHash = Animator.StringToHash("isAttacking");
    private static int isAttackingTriggerHash = Animator.StringToHash("isAttackingTrigger");
    private static int attackIndexHash = Animator.StringToHash("attackIndex");
    private static int velocityHash = Animator.StringToHash("velocity");
    private static int yawHash = Animator.StringToHash("direction");
    private static int pitchHash = Animator.StringToHash("pitch");
    private static int rollHash = Animator.StringToHash("roll");
    //private static int attacking01AnimHash = Animator.StringToHash("Base Layer.Attack");
    //private static int attackingShakeAnimHash = Animator.StringToHash("Base Layer.AttackShake");
    #endregion



    void Start()
    {
        sharkAnim = GetComponent<Animator>();
        sharkRb = GetComponent<Rigidbody>();
        biteDetector.SetActive(false);
        meatDetector.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateMachine();
        if (disableAttacking)
            return;
        Attacking();
    }


    private void AnimatorStateMachine()
    {
        
        forward = Input.GetAxis("Vertical");
        sharkAnim.SetFloat(yawHash, yaw);
        sharkAnim.SetFloat(pitchHash, pitch);
        

        if (rollSpeed > 0) sharkAnim.SetFloat(rollHash, roll);

        if (forward > 0.1 || forward < -0.1 || yaw > 0.1 || yaw < -0.1 || pitch > 0.1 || pitch < -0.1)
        {
            isMoving = true;
            sharkAnim.SetBool(isMovingHash, true);
        }
        else
        {
            isMoving = false;
            sharkAnim.SetBool(isMovingHash, false);
        }

        if (rollSpeed > 0 && (roll > 0.1 || roll < -0.1))
        {
            isRolling = true;
            sharkAnim.SetBool(isRollingHash, true);
        }
        else
        {
            isRolling = false;
            sharkAnim.SetBool(isRollingHash, false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && (forward > 0.1f || forward < -0.1f))
        {
            animatorVelocity = Mathf.Lerp(animatorVelocity, animatorVelocityWithShift, normalToShiftVelocityTime * Time.deltaTime);
            actualForward = Mathf.Lerp(actualForward, forwardShiftMaxSpeed, normalToShiftVelocityTime * Time.deltaTime);
        }
        else
        {
            animatorVelocity = Mathf.Lerp(animatorVelocity, 1, normalToShiftVelocityTime * Time.deltaTime);
            actualForward = Mathf.Lerp(actualForward, forward, normalToShiftVelocityTime);
        }
        sharkAnim.SetFloat(velocityHash, animatorVelocity);
    }

    private void Attacking()
    {
        if (!isWhaleShark)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                int attackIndex = UnityEngine.Random.Range(0, attackAnimation + 1);
                sharkAnim.SetTrigger(isAttackingTriggerHash);
                sharkAnim.SetInteger(attackIndexHash, attackIndex);
            }
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
                sharkAnim.SetBool(isAttackingHash, true);
            else
                sharkAnim.SetBool(isAttackingHash, false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                timeStartAbsorb = Time.time;
                Debug.Log("Absorbing");
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
                sharkAnim.SetBool(isAttackingHash, false);

            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                sharkAnim.SetBool(isAttackingHash, true);

                float totalTimeAbsorb = Time.time - timeStartAbsorb;
                if (totalTimeAbsorb > timeAbsorbing)
                    sharkAnim.SetBool(isAttackingHash, false);
            }
        }
    }
}
