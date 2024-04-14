using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        // Player ������Ʈ �������� �ڽ� ������Ʈ��
        // arm_ ~ rifle_01 ������Ʈ�� Animator ������Ʈ�� ����
        anim = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }
    public void Play(string stateName, int layer, float normalizedTime)
    {
        anim.Play(stateName, layer, normalizedTime);
    }
}
