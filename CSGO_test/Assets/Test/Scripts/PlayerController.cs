using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeWalk = KeyCode.LeftShift;        // 달리기 키
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;            // 점프 키

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;                        // 걷기 소리
    [SerializeField]
    private AudioClip audioClipRun;                         // 달리기 소리

    private RotateToMouse               rotateToMouse;      // 카메라 회전
    private CharaMovementController     movement;           // 플레이어 이동
    private Status                      status;             // 플레이어 상태전환
    private PlayerAnimationController   anim;               // 애니메이션 재생 제어
    private AudioSource                 audioSource;        // 사운드 재생 제어
    private WeaponAssaultRifle          weapon;             // 무기를 이용한 공격 제어

    private void Awake()
    {
        Cursor.visible      = false;                        // 커서 비활성화
        Cursor.lockState    = CursorLockMode.Locked;        // 커서 잠금

        rotateToMouse       = GetComponent<RotateToMouse>();
        movement            = GetComponent<CharaMovementController>();
        status              = GetComponent<Status>();
        anim                = GetComponent<PlayerAnimationController>();
        audioSource         = GetComponent<AudioSource>();
        weapon              = GetComponent<WeaponAssaultRifle>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }
    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(x != 0 || z != 0)
        {
            bool isWalk = false;

            if (Input.GetKey(keyCodeWalk)) isWalk = true;

            movement.MoveSpeed  = isWalk == true ? status.WalkSpeed : status.RunSpeed;
            anim.MoveSpeed      = isWalk == true ? 0.5f : 1;
            audioSource.clip    = isWalk == true ? audioClipWalk : audioClipRun;

            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

        }
        else 
        { 
            movement.MoveSpeed  = 0;
            anim.MoveSpeed      = 0;
            if(audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }
    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
    private void UpdateWeaponAction()
    {
        if(Input.GetMouseButton(0))
        {
            weapon.StartWeaponAction();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }
    }
}
