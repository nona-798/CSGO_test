using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        // Player 오브젝트 기준으로 자식 오브젝트인
        // arm_ ~ rifle_01 오브젝트에 Animator 컴포넌트가 있음
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
