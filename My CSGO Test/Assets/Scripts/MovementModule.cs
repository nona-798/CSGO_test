using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class MovementModule : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;   // 이동 속도
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float Gravity;
    private Vector3 moveDir;   // 이동 방향
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController characterController;    // 플레이어 이동 제어를 위한 컴포넌트
    
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
        // 이동할 방향 = 캐릭터 회전 값 * 입력 방향 값
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        // 이동벡터 값 = 이동 방향 * 속도
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
