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
    // Reload 트리거 함수
    public void OnReload()
    {
        anim.SetTrigger("OnReload");
    }
    // Movement 분기점 변수
    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }
    // Reload 분기점 변수
    public float Ammo
    {
        set => anim.SetFloat("Ammo", value);
        get => anim.GetFloat("Ammo");
    }
    // Assault Rifle 마우스 오른쪽 클릭 액션 (default/ aim mode)
    public bool AimModeIs
    {
        set => anim.SetBool("IsAimMode", value);
        get => anim.GetBool("IsAimMode");
    }
    public void Play(string stateName, int layer, float normalizedTime)
    {
        anim.Play(stateName, layer, normalizedTime);
    }
    // 현재 재생중인 애니메이션 Get 이름 bool함수
    public bool CurrentAnimationIs(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
