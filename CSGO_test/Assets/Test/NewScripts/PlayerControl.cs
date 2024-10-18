using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeWalk = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]
    private KeyCode keyCodeCrouch = KeyCode.RightControl;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;

    [Header("Rotate Cam")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject prefabPlayer;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject spine;

    [Header("Modules")]
    private RotateMouse rotMouse;
    private IMovement movement;
    private IStatus status;
    private PlayerAnimControl anim;
    private AudioSource audioSource;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotMouse = GetComponent<RotateMouse>();
        movement = GetComponent<IMovement>();
        status = GetComponent<IStatus>();
        anim = GetComponent<PlayerAnimControl>();
        audioSource = GetComponent<AudioSource>();
        
    }
    // Update is called once per frame
    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
    }
    private void LateUpdate()
    {
        //anim.BodyXRotate(target);
    }
    /// <summary> 마우스로 카메라 회전 </summary>
    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

        rotMouse.UpdateRotate(cam.gameObject, mouseX, mouseY);
        //rotMouse.UpdateBodyXRotate(anim.PlayerSpineRot, mouseY);
        
    }
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중
        if( z != 0 || x != 0 )
        {
            bool isWalk = false;

            if(Input.GetKey(keyCodeWalk))
            {
                isWalk = true;
            }
            
            movement.MoveSpeed = isWalk == true ? status.WalkSpeed : status.RunSpeed;
            if(movement.MoveSpeed > 0)anim.OnMove();
            anim.MoveSpeed = isWalk == true ? 3 : 5;
            anim.MoveForward = z;
            anim.MoveStrafe = x;
        }
        // 제자리
        else
        {
            movement.MoveSpeed = 0;
            anim.MoveSpeed = anim.MoveForward = anim.MoveStrafe = 0;
            if(movement.MoveSpeed == 0)anim.OnIdle();
        }
        
        movement.MoveTo(new Vector3(x, 0, z));
    }
    private void UpdateJump()
    {
        if(Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
}
