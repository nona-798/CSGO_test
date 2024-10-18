using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class IMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector3 moveForce;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity;
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }
    public Vector3 MoveForce
    {
        set => moveForce = new Vector3(Mathf.Max(0, value.x), Mathf.Max(0, value.y), Mathf.Max(0, value.z));
        get => moveForce;
    }

    private CharacterController charaCon;
    private void Awake()
    {
        charaCon = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(!charaCon.isGrounded)
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
