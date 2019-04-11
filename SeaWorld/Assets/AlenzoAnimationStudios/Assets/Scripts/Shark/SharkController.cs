using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

[RequireComponent(typeof(PlayerStats))]

public class SharkController : MonoBehaviour
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
                     public  GameObject biteParent;
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
    //private CapsuleCollider sharkCapsuleCollider;
    private PlayerStats sharkStats;

    private FlockAI myFlockAI;

    #region AnimatorParameters
    private float animatorVelocity = 1;
    private bool isMoving = false;
    private bool isRolling = false;
    private float forward = 0;
    private float actualForward = 0;
    public float yaw = 0;
    public float pitch = 0;
    public float roll = 0;
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

    void Start ()
    {
        sharkAnim = GetComponent<Animator>();
        sharkRb = GetComponent<Rigidbody>();
        //sharkCapsuleCollider = GetComponent<CapsuleCollider>();
        //sharkAnimStateInfo = sharkAnim.GetCurrentAnimatorStateInfo(0);
        sharkStats = GetComponent<PlayerStats>();

        biteDetector.SetActive(false);
        meatDetector.SetActive(false);

        // Setting rigidbody
        //sharkRb.useGravity = false;
        //sharkRb.drag = 1f;
        //sharkRb.angularDrag = 1f;
        //sharkRb.constraints = RigidbodyConstraints.FreezeRotationZ;

        myFlockAI = GetComponent<FlockAI>();
}
	
	void Update ()
    {
        if (myFlockAI.CanMove && myFlockAI.isInFlock)
        {
            AnimatorStateMachine();
            if (disableAttacking)
                return;
            Attacking();
        }
        
    }
    private void AnimatorStateMachine()
    {
        //sharkAnimStateInfo = sharkAnim.GetCurrentAnimatorStateInfo(0);
        forward = Input.GetAxis("Vertical");
        if (!mouseRotationControlled)
        {
            yaw = Input.GetAxis("Horizontal") * 0.4f;
            pitch = Input.GetAxis("Pitch") * 0.4f;
            roll = Input.GetAxis("Roll") * 0.4f;

            sharkAnim.SetFloat(yawHash, yaw);
            sharkAnim.SetFloat(pitchHash, pitch);
        }
        else
        {
            if (righMousePressedToRotate)
            {
                if (Input.GetMouseButton(1))
                {
                    //float screenWith = Screen.width / 2;
                    //float screenHeight = Screen.height / 2;
                    //float xClampMousePos = ((Input.mousePosition.x - screenWith) / (screenWith)) * 1.8f;
                    //float yClampMousePos = ((Input.mousePosition.y - screenHeight) / (screenHeight)) * -1.8f;
                    //xClampMousePos = Mathf.Clamp(xClampMousePos, -1f, 1f);
                    //yClampMousePos = Mathf.Clamp(yClampMousePos, -1f, 1f);
                    //yaw = Mathf.Lerp(yaw, xClampMousePos, Time.deltaTime * 10);
                    //pitch = Mathf.Lerp(pitch, yClampMousePos, Time.deltaTime * 10);



                    //yaw += Input.GetAxis("Mouse X") * 0.1f;
                    //pitch -= Input.GetAxis("Mouse Y") * 0.1f;
                    //yaw = Mathf.Clamp(yaw, -1f, 1f);
                    //pitch = Mathf.Clamp(pitch, -1f, 1f);
                    FlockManager.Instance.SetPitchAndYaw();
                    yaw = FlockManager.Instance.yaw;
                    pitch = FlockManager.Instance.pitch;
                }
                else
                {
                    yaw = Mathf.Lerp(yaw, 0f, Time.deltaTime * rotateStopMultMouse);
                    pitch = Mathf.Lerp(pitch, 0f, Time.deltaTime * rotateStopMultMouse);
                }
            }
            else
            {
                yaw += Input.GetAxis("Mouse X") * 0.1f;
                pitch -= Input.GetAxis("Mouse Y") * 0.1f;
                yaw = Mathf.Clamp(yaw, -1f, 1f);
                pitch = Mathf.Clamp(pitch, -1f, 1f);
            }
            sharkAnim.SetFloat(yawHash, yaw);
            sharkAnim.SetFloat(pitchHash, pitch);
        }

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

        if ( rollSpeed > 0 && (roll > 0.1 || roll < -0.1))
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

    private void FixedUpdate()
    {
        AutoXZalign();
        if (disableSwimming)
            return;
        Locomotion();
    }

    private void AutoXZalign()
    {
        if (autoPitchRollAlign && !isMoving)
        {
            Vector3 xzAlignVector = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            Quaternion desireRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xzAlignVector), autoPitchRollSpeed);
            sharkRb.MoveRotation(desireRotation);
        }
    }

    private void Locomotion()
    {
        if (isMoving || isRolling)
        {
            Vector3 forwardMovement = transform.position + transform.forward * actualForward * swimmingSpeedMult;
            Vector3 cohesionMove = myFlockAI.CalculateCohesion();
            sharkRb.MovePosition(forwardMovement + 0.05f * cohesionMove);
            Vector3 deltaRotation;

            if (!mouseRotationControlled)
            {
                deltaRotation = new Vector3(pitch * pitchSpeed, yaw * yawSpeed, -roll * rollSpeed) * rotateShiftMultKeyboard;
                sharkRb.MoveRotation(sharkRb.rotation * Quaternion.Euler(deltaRotation));
            }
            else
            {
                deltaRotation = new Vector3(pitch, yaw, 0) * rotateShiftMultMouse;

                Quaternion desireRotation = sharkRb.rotation * Quaternion.Euler(deltaRotation);
                desireRotation = Quaternion.Euler(new Vector3(desireRotation.eulerAngles.x, desireRotation.eulerAngles.y, 0));
                if (desireRotation.eulerAngles.x >= 45 && desireRotation.eulerAngles.x < 180)
                    desireRotation = Quaternion.Euler(new Vector3(45, desireRotation.eulerAngles.y, 0));
                if (desireRotation.eulerAngles.x > 180 && desireRotation.eulerAngles.x <= 315)
                    desireRotation = Quaternion.Euler(new Vector3(315, desireRotation.eulerAngles.y, 0));
                if (desireRotation.eulerAngles.y >= 90 && desireRotation.eulerAngles.y < 135)
                    desireRotation = Quaternion.Euler(new Vector3(desireRotation.eulerAngles.x, 90, 0));
                if (desireRotation.eulerAngles.y >= 135 && desireRotation.eulerAngles.y <= 270)
                    desireRotation = Quaternion.Euler(new Vector3(desireRotation.eulerAngles.x, 270, 0));
                sharkRb.MoveRotation(Quaternion.Lerp(sharkRb.rotation, desireRotation, 0.6f));

                //Vector3 relativePos = (raycastHit.point - sharkRb.transform.position).normalized;
                //Quaternion desireRotation = Quaternion.LookRotation(relativePos, sharkRb.transform.up);
                //desireRotation.eulerAngles = new Vector3(desireRotation.eulerAngles.x, desireRotation.eulerAngles.y, 0);
                //sharkRb.rotation = Quaternion.Lerp(sharkRb.rotation, desireRotation, Time.deltaTime * rotateShiftMultMouse);
                //Limit the rotation, very important for not get a weir result.


                //if (sharkRb.rotation.eulerAngles.x >= 45 && sharkRb.rotation.eulerAngles.x <180)
                //    sharkRb.transform.rotation = Quaternion.Euler(new Vector3(45, sharkRb.rotation.eulerAngles.y, sharkRb.rotation.eulerAngles.z));
                //if (sharkRb.rotation.eulerAngles.x >= 180 && sharkRb.rotation.eulerAngles.x <= 315)
                //    sharkRb.transform.rotation = Quaternion.Euler(new Vector3(315, sharkRb.rotation.eulerAngles.y, sharkRb.rotation.eulerAngles.z));
            }
        }
        else
            sharkRb.velocity = Vector3.Lerp(sharkRb.velocity, Vector3.zero, Time.deltaTime * 1f);
    }

    #region Be_Bitten
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BiteDetector"))
        {
            if (!beenBiteFlag)
            {
                beenBiteFlag = true;
                CancelInvoke("ResetBeenBite");
                Invoke("ResetBeenBite", 0.2f);
                attackerOb = other.gameObject.GetComponent<BiteDectector>();
                if (allowParentToAttacker)
                    ParentToAttacker();
                if (!isInmortal)
                {
                    if (attackerOb.Baiting)
                        sharkStats.ReduceHealt(attackerOb.GetDamage());
                    else if (attackerOb.BaitSqueezing)
                        sharkStats.ReduceHealt(attackerOb.GetDamageWithSqueeze());
                }
            }
        }
    }

    private void ResetBeenBite()
    {
        beenBiteFlag = false;
    }

    private void ParentToAttacker()
    {
        if (sharkStats.Weight <= attackerOb.transform.root.GetComponent<PlayerStats>().Weight)
        {
            float prob = UnityEngine.Random.Range(0f, 1f);
            stickToBiteFlag = (prob > chanceToLetGo) ? true : false;

            if (stickToBiteFlag)
            {
                transform.SetParent(attackerOb.transform.root.GetComponent<SharkController>().biteParent.transform);
                Invoke("DisableParenting", .2f);
            }
        }
    }

    private void DisableParenting()
    {
        transform.SetParent(null);
    }
    #endregion

    #region EventFuntions
    public void DoIdle() //Called in idle animation
    {
        float doNewIdleProb = Random.Range(0f, 1f);
        if (doNewIdleProb < idleChangeProbability)
        {
            float randomTime = Random.Range(0f, 2f);
            Invoke("DoIdleInTime", randomTime);
        }
    }

    private void DoIdleInTime()
    {
        int newIdleIndex = Random.Range(1, idleCountAnim);
        sharkAnim.SetBool(doIdleHash, true);
        sharkAnim.SetInteger(idleRandomHash, newIdleIndex);
    }

    //sharkAnimStateInfo

    public void AttackEvent() //Called in attack animation
    {
        biteDetector.SetActive(true);
        biteDetector.GetComponent<BiteDectector>().Baiting = true;
        meatDetector.SetActive(true);

        Invoke("DisableAttack", 0.1f);
    }

    public void AttackSharkWhale()
    {
        meatDetector.SetActive(true);
    }

    public void AttackSqueeze() //Called in attack shake loop animation
    {
        biteDetector.SetActive(true);
        biteDetector.GetComponent<BiteDectector>().BaitSqueezing = true;

        Invoke("DisableAttack", 0.2f);
    }

    public void DisableAttack() //Called by Invoke methop. Also called in the attack whale shark (at the end)
    {
        biteDetector.SetActive(false);
        biteDetector.GetComponent<BiteDectector>().Baiting = false;
        biteDetector.GetComponent<BiteDectector>().BaitSqueezing = false;
        meatDetector.SetActive(false);
    }
    #endregion
}
