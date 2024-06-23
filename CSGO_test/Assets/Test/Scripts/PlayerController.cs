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
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;              // ������ Ű

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;                        // �ȱ� �Ҹ�
    [SerializeField]
    private AudioClip audioClipRun;                         // �޸��� �Ҹ�

    private RotateToMouse               rotateToMouse;      // ī�޶� ȸ��
    private CharaMovementController     movement;           // �÷��̾� �̵�
    private Status                      status;             // �÷��̾� ������ȯ
    private AudioSource                 audioSource;        // ���� ��� ����
    private WeaponBase                  weapon;             // ���⸦ �̿��� ���� ����

    private void Awake()
    {
        Cursor.visible      = false;                        // Ŀ�� ��Ȱ��ȭ
        Cursor.lockState    = CursorLockMode.Locked;        // Ŀ�� ���

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

        // �̵����� ��
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

            // ����Ű �Է� ���δ� �� ������ Ȯ���ϱ� ������
            // ������� ���� �ٽ� ������� �ʵ��� isPlaying���� üũ�ؼ� ���
            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

        }
        // ���ڸ��� �������� ��
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
