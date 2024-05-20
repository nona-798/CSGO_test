using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTransform : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    
    // �̵� ������ �����Ǹ� �˾Ƽ� �̵��ϵ��� ��
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // �ܺο��� �Ű������� �̵� ������ ����
    public void MoveTo(Vector3 dir)
    {
        moveDirection = dir;
    }
}
