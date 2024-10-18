using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class MovementModule : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;   // �̵� �ӵ�
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float Gravity;
    private Vector3 moveDir;   // �̵� ����
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController characterController;    // �÷��̾� �̵� ��� ���� ������Ʈ
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if (!characterController.isGrounded)
        {
            moveDir.y += Gravity * Time.deltaTime;
        }
            characterController.Move(moveDir * Time.deltaTime);
    }
    private float ControlSpeed()
    {
        if (!characterController.isGrounded)
        {
            moveSpeed *= 0.55f;
        }
        return moveSpeed;
    }
    public void MoveTo(Vector3 dir)
    {
        // �̵��� ���� = ĳ���� ȸ�� �� * �Է� ���� ��
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        // �̵����� �� = �̵� ���� * �ӵ�
        moveDir = new Vector3(dir.x * ControlSpeed(), moveDir.y, dir.z * ControlSpeed());
    }
    public void Jump()
    {
        if (characterController.isGrounded)
        {
            moveDir.y = jumpForce;
        }
    }
}
