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
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;              // 재장전 키

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;                        // 걷기 소리
    [SerializeField]
    private AudioClip audioClipRun;                         // 달리기 소리

    private RotateToMouse               rotateToMouse;      // 카메라 회전
    private CharaMovementController     movement;           // 플레이어 이동
    private Status                      status;             // 플레이어 상태전환
    private AudioSource                 audioSource;        // 사운드 재생 제어
    private WeaponBase                  weapon;             // 무기를 이용한 공격 제어

    private void Awake()
    {
        Cursor.visible      = false;                        // 커서 비활성화
        Cursor.lockState    = CursorLockMode.Locked;        // 커서 잠금

        rotateToMouse       = GetComponent<RotateToMouse>();
        movement            = GetComponent<CharaMovementController>();
        status              = GetComponent<Status>();
        audioSource         = GetComponent<AudioSource>();
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

        // 이동중일 때
        if(x != 0 || z != 0)
        {
            bool isWalk = false;

            if (Input.GetKey(keyCodeWalk))
            {
                isWalk = true;
            }

            movement.MoveSpeed  = isWalk == true ? status.WalkSpeed : status.RunSpeed;
            weapon.Animator.MoveSpeed = isWalk == true ? 1.4f : 3.0f;
            audioSource.clip    = isWalk == true ? audioClipWalk : audioClipRun;

            // 방향키 입력 여부는 매 프레임 확인하기 때문에
            // 재생중일 때는 다시 재생하지 않도록 isPlaying으로 체크해서 재생
            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

        }
        // 제자리에 멈춰있을 때
        else 
        { 
            movement.MoveSpeed  = 0.0f;
            weapon.Animator.MoveSpeed = 0.0f;
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
        if(Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction(0);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }


        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }
    public void TakeDamage(int dmg)
    {
        bool isDie = status.DecreaseHP(dmg);

        if(isDie == true)
        {
            Debug.Log("GameOver");
        }
    }
    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
}
