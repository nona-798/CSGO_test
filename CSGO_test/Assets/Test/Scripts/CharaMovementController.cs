using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class CharaMovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;    // 이동 속도
    private Vector3 moveForce;  // 이동 방향
    [SerializeField]
    private float jumpForce;    // 점프 힘
    [SerializeField]
    private float gravity;      // 중력 힘

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController charaCon;
    // Start is called before the first frame update
    private void Awake()
    {
        charaCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!charaCon.isGrounded) // 허공에 떠 있으면 중력만큼 y축 이동 속도 감소
        {
            moveForce.y += gravity * Time.deltaTime;
        }
        charaCon.Move(moveForce * Time.deltaTime);
    }
    public void MoveTo(Vector3 dir)
    {
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);
        moveForce = new Vector3(dir.x * moveSpeed, moveForce.y, dir.z * moveSpeed);
    }
    public void Jump()
    {
        if(charaCon.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
