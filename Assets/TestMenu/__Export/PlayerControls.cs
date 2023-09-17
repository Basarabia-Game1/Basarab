using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_SWITCH
using Rewired;
#elif UNITY_ANDROID || UNITY_IOS
using UniversalMobileController;
#endif

public class PlayerControls : MonoBehaviour
{
    //public List<GameObject> itemList;

    // [SerializeField]
    // private Image canvas_ItemSelect;
    // [SerializeField]
    // private List<Sprite> itemcurrentselect;
#if UNITY_ANDROID || UNITY_IOS
    [SerializeField]
    private Controll_Items controll_Items_info;
    [SerializeField] private FloatingJoyStick joystick;
#endif

    public float MaxMoveSpeed = 8;
    public float MaxSprintSpeed = 8;

    // public AudioSource DashSound;
    // public AudioSource StepSound;

    private CharacterController controllerComponent;
    public Animator animatorComponent;
    //private PlayerGrabTrigger grabTrigger;

    private Vector3 moveSpeed;
    private float grabCooldown;
    private float dashingTimeLeft;

    private static readonly int AtackParam = Animator.StringToHash("Atack");
    private static readonly int GrabParam = Animator.StringToHash("grab");
    private static readonly int GatheringParam = Animator.StringToHash("Gathering");
    private static readonly int WalkSpeedParam = Animator.StringToHash("walk speed");
    private static readonly int MoveParam = Animator.StringToHash("Move");
    private static readonly int SprintParam = Animator.StringToHash("Sprint");
    private static readonly int SprintEndParam = Animator.StringToHash("SprintEnd");
    private static readonly int MiningParam = Animator.StringToHash("Mining");

    private bool _isMove = true;
    private bool _isPickUp = false;
    private bool Sprint;
    private float Speed;

    private void Start()
    {
        animatorComponent = GetComponent<Animator>();
        controllerComponent = GetComponent<CharacterController>();

        // grabTrigger = GetComponentInChildren<PlayerGrabTrigger>();
        // SetItemPlayer(grabTrigger.curentItemSelect);
    }

    private void Update()
    {
        // if (MainMenu.IsGameStarted)
        // {
#if UNITY_SWITCH && !UNITY_EDITOR
            if (ReInput.players.GetPlayer(0).GetButton("Sprint"))
#elif UNITY_ANDROID || UNITY_IOS
            if (Sprint)
#else
            if (Input.GetKey(KeyCode.LeftShift))
#endif
                Speed = MaxSprintSpeed;
            else
                Speed = MaxMoveSpeed;

            UpdateWalk();

#if UNITY_SWITCH && !UNITY_EDITOR
            if (ReInput.players.GetPlayer(0).GetButtonDown("Action"))
            {
                Grab();
            }
            else if (ReInput.players.GetPlayer(0).GetButtonDown("Open"))
                grabTrigger.OpenBuild();

            //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) 
            //    Dash(false);
            //else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) 
            //    Dash(true);

            if (controll_Items_info.chest || controll_Items_info.workbench)
                if (ReInput.players.GetPlayer(0).GetButtonDown("Back"))
                    controll_Items_info.ClouseChestWorkbench();
            
            //if (ReInput.players.GetPlayer(0).GetButtonDown("Back"))
            //    PlayerAtack();
#else
            if (Input.GetKeyDown(KeyCode.Space))
                Grab();
            // else if (Input.GetKeyDown(KeyCode.E))
            //     grabTrigger.OpenBuild();

            //if (Input.GetMouseButtonDown(0))
            //    PlayerAtack();

            if (Input.GetKeyDown(KeyCode.X))
                Dash(false);
            else if (Input.GetKey(KeyCode.X))
                Dash(true);

            // if (controll_Items_info.chest || controll_Items_info.workbench)
            //     if (Input.GetKeyDown(KeyCode.Escape))
            //         controll_Items_info.ClouseChestWorkbench();
#endif
            grabCooldown -= Time.deltaTime;
        //}

        animatorComponent.SetFloat(WalkSpeedParam, new Vector3(moveSpeed.x, 0, moveSpeed.z).magnitude);
    }


    public void SetItemPlayer(int itemCount)
    {
        // for (int i = 0; i < itemList.Count; i++)
        // {
        //     if (itemCount == i)
        //     {
        //         itemList[i].SetActive(true);
        //         canvas_ItemSelect.sprite = itemcurrentselect[i];
        //         //grabTrigger.curentItemSelect = i;
        //     }
        //     else
        //         itemList[i].SetActive(false);
        // }
    }
    private void Dash(bool holding)
    {
        if (!_isMove)
            return;

        if (dashingTimeLeft < (holding ? -.4f : -.2f))
        {
            dashingTimeLeft = .3f;
            // DashSound.Play();
        }
    }

    public void StepAnimationCallback()
    {
        if (dashingTimeLeft > 0) return;

        // if (StepSound.pitch < 1) StepSound.pitch = Random.Range(1.05f, 1.15f);
        // else StepSound.pitch = Random.Range(0.9f, 0.95f);
        // StepSound.Play();
    }

    public void MobileGrap()
    {
        Grab();
    }
    public void MobileOpenBuilds()
    {
        //grabTrigger.OpenBuild();
    }
    public void MobileSprint()
    {
        Sprint = !Sprint;
    }

    private void UpdateWalk()
    {
        if (!_isMove)
            return;
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        if (dashingTimeLeft <= 0)
        {

            float horizontalInput;
            float verticalInput;

#if UNITY_SWITCH && !UNITY_EDITOR
            horizontalInput = ReInput.players.GetPlayer(0).GetAxis("MoveHorizontal");
            verticalInput = ReInput.players.GetPlayer(0).GetAxis("MoveVertical");
#elif UNITY_ANDROID && !UNITY_EDITOR || UNITY_IOS && !UNITY_EDITOR
            horizontalInput = joystick.GetHorizontalValue();
            verticalInput = joystick.GetVerticalValue();
#else
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
#endif
            Vector3 target = Speed * new Vector3(horizontalInput, 0, verticalInput).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);

            if (moveSpeed.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed),
                    Time.deltaTime * 720);
            }
            if (moveSpeed.magnitude > 0.3f)
                animatorComponent.SetTrigger(MoveParam);

        }
        else
        {
            moveSpeed = MaxMoveSpeed * 5 * moveSpeed.normalized;
        }

        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }

    private void PlayerAtack()
    {
        if (grabCooldown > 0) return;

        animatorComponent.SetTrigger(AtackParam);

        // Collider[] isInSphere = Physics.OverlapSphere(grabTrigger.transform.position,
        //     grabTrigger.GrabRadius, LayerMask.NameToLayer("Enemy"));
        // if (isInSphere.Length > 0)
        // {
        //     Rigidbody target = isInSphere[0].attachedRigidbody;
        //     Debug.LogError($"Target -> {target.gameObject.name}");
        //     if (!target.isKinematic || target.gameObject.tag == "Enemy")
        //     {
        //         Debug.LogError($"Target -> Enemy");
        //         target.GetComponent<EnemyControl>().DestroyEnenyDeath(0);
        //         return;
        //     }
        // }
        grabCooldown = .5f;

    }
    private void Grab()
    {
        if (!_isMove) return;
        if (grabCooldown > 0) return;


        //if (grabTrigger.GrabbedObject != null)
        //{
        //    StartAnimPickUp();
        //    //grabTrigger.Release();
        //    return;
        //}

        //grabTrigger.Grab();
        //animatorComponent.SetTrigger(GrabParam);

        grabCooldown = .5f;
    }

    public void AnimMoveActive()
    {
        _isMove = !_isMove;
    }
    public void StartAnimPickUp()
    {
        animatorComponent.SetTrigger(GatheringParam);
    }

    public void StartAnimGrab()
    {
        animatorComponent.SetTrigger(GrabParam);
    }
    public void StartAnimAtack()
    {
        animatorComponent.SetTrigger(AtackParam);
    }

    public void StartAnimMining()
    {
        animatorComponent.SetTrigger(MiningParam);
    }

    public void GrabAnimationCallback()
    {
        //grabTrigger.TreeGrab();
    }
    public void PickUpAnimationCallback()
    {
        if (_isPickUp)
        {
            //grabTrigger.Release();
            _isPickUp = !_isPickUp;
        }
        else
        {
            //grabTrigger.PickUpObject();
            _isPickUp = !_isPickUp;
        }
    }

}