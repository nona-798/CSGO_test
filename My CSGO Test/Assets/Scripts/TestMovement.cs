using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class TestMovement : MonoBehaviour
{
    public Transform follower; // ������ �Ӹ�
    public float dia; // ������
    private List<Transform> followers; // ������ ����
    [SerializeField]
    private List<Vector3> followerPos; // ������ �� ���� ��ġ

    private float front;    // �յ� �̵� ��
    private float strafe;   // �¿� �̵� ��

    private int snakeNum = 4;

    [SerializeField]
    private float moveSpeed; // �̵� �ӵ�

    private CharacterController characterController;
    // Start is called before the first frame update
    private void Awake()
    {
        followerPos.Add(follower.position);
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        front = Input.GetAxisRaw("Vertical");
        strafe = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(strafe, 0, front) * moveSpeed * Time.deltaTime;
    }
    private void LateUpdate()
    {
        makeSnakeFollow();
    }
    private void makeSnakeFollow()
    {
        float dis = ((Vector3)follower.position - followerPos[0]).magnitude;
        if(dis > dia)
        {
            Vector3 dir = ((Vector3)follower.position - followerPos[0]).normalized;
            followerPos.Insert(0, followerPos[0] + dir * dia);
            followerPos.RemoveAt(followerPos.Count - 1);

            dis -= dia;
        }
        for(int i = 0; i < followers.Count; i++)
        {
            followers[i].position = Vector3.Lerp(followerPos[i + 1], followerPos[i], dis / dia);
        }
    }

    public void AddFollower()
    {
        Transform follow = Instantiate(follower, followerPos[followerPos.Count - 1], Quaternion.identity, transform);
        followers.Add(follow);
        followerPos.Add(follower.position);
    }
    // ���ӵ� �ϴ� ����
    //public void MoveTo(Vector3 wishDir)
    //{
    //    Vector3 dir = transform.rotation * new Vector3(wishDir.x, 0, wishDir.z);
    //
    //    
    //}
    //private float DotProduct(Vector3 dir1, Vector3 dir2)
    //{
    //    float target = Vector3.Dot(dir1, dir2);
    //    return target;
    //}
}
