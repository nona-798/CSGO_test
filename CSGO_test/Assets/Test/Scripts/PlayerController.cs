using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeWalk = KeyCode.LeftShift;        // �޸��� Ű
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;            // ���� Ű

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;                        // �ȱ� �Ҹ�
    [SerializeField]
    private AudioClip audioClipRun;                         // �޸��� �Ҹ�

    private RotateToMouse               rotateToMouse;      // ī�޶� ȸ��
    private CharaMovementController     movement;           // �÷��̾� �̵�
    private Status                      status;             // �÷��̾� ������ȯ
    private PlayerAnimationController   anim;               // �ִϸ��̼� ��� ����
    private AudioSource                 audioSource;        // ���� ��� ����
    private WeaponAssaultRifle          weapon;             // ���⸦ �̿��� ���� ����

    private void Awake()
    {
        Cursor.visible      = false;                        // Ŀ�� ��Ȱ��ȭ
        Cursor.lockState    = CursorLockMode.Locked;        // Ŀ�� ���

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
