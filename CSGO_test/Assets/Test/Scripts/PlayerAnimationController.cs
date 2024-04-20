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
    // Reload Ʈ���� �Լ�
    public void OnReload()
    {
        anim.SetTrigger("OnReload");
    }
    // Movement �б��� ����
    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }
    // Reload �б��� ����
    public float Ammo
    {
        set => anim.SetFloat("Ammo", value);
        get => anim.GetFloat("Ammo");
    }
    // Assault Rifle ���콺 ������ Ŭ�� �׼� (default/ aim mode)
    public bool AimModeIs
    {
        set => anim.SetBool("IsAimMode", value);
        get => anim.GetBool("IsAimMode");
    }
    public void Play(string stateName, int layer, float normalizedTime)
    {
        anim.Play(stateName, layer, normalizedTime);
    }
    // ���� ������� �ִϸ��̼� Get �̸� bool�Լ�
    public bool CurrentAnimationIs(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
