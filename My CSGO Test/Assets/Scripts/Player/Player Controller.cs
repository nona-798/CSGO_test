using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCode")]
    [SerializeField]
    private KeyCode keyCodeSneak = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode keyCodeCrouch = KeyCode.LeftControl;
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;

    [Header("Main Camera")]
    [SerializeField]
    private GameObject Cam;
    [SerializeField]
    private GameObject Head;

    [Header("AudioClips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;

    private RotateToMouse rotToMouse;
    private MovementModule movement;
    private PlayerStatus playerStatus;
    private PlayerAnimatorController playerAnim;
    private WeaponBase weapon;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementModule>();
        playerStatus = GetComponent<PlayerStatus>();
        playerAnim = GetComponentInChildren<PlayerAnimatorController>();
    }
    private void Update()
    {
        UpdateRotate();
        UpdateStand();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }
    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

        rotToMouse.UpdateRotate(Cam, mouseX, mouseY);
        
        Cam.transform.position = Head.transform.position;
    }
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if( z != 0 || x != 0)
        {
            bool isWalk = false;
            if (Input.GetKey(keyCodeSneak)) isWalk = true;

            movement.MoveSpeed = isWalk == true ? playerStatus.WalkSpeed : playerStatus.RunSpeed;
            //audioSource.clip = isWalk == true ? audioClipWalk : audioClipRun;
            //audioSource.Play();
            playerAnim.MoveSpeed = isWalk == true ? 1 : 2;
            weapon.Animator.MoveSpeed = isWalk == true ? 1 : 2;
            playerAnim.DirX = isWalk == true ? x : x * 2;
            playerAnim.DirZ = isWalk == true ? z : z * 2;
        }
        else
        {
            movement.MoveSpeed = 0;
            playerAnim.MoveSpeed = 0;
            weapon.Animator.MoveSpeed = 0;
            playerAnim.DirX = playerAnim.DirX = 0;
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
    private void UpdateStand()
    {
        if (Input.GetKey(keyCodeCrouch))
        {
            playerAnim.IsStand(false);
        }
        else if(Input.GetKeyUp(keyCodeCrouch))
        {
            playerAnim.IsStand(true);
        }
    }
    private void UpdateWeaponAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction(0);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction(0);
        }
        if(Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }
    public void TakeDamage(int damage)
    {
        bool isDie = playerStatus.DeaceaseHP(damage);
        if(isDie == true)
        {
            Debug.Log("Player Dead");
        }
    }
    public void SwitchWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
}
