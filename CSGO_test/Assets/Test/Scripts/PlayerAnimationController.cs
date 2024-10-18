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

    /// <summary> Reload Ʈ���� �Լ� </summary>
    public void OnReload()
    {
        anim.SetTrigger("OnReload");
    }

    ///  <summary> Empty Ʈ���� �Լ� </summary>
    public void OnEmpty()
    {
        anim.SetTrigger("OnEmpty");
    }

    /// <summary> Movement �б��� ���� </summary>
    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }

    /// <summary> Reload �б��� ���� </summary>
    public float Ammo
    {
        set => anim.SetFloat("Ammo", value);
        get => anim.GetFloat("Ammo");
    }

    /// <summary> Assault Rifle ���콺 ������ Ŭ�� �׼� (default/ aim mode) </summary>
    public bool AimModeIs
    {
        set => anim.SetBool("IsAimMode", value);
        get => anim.GetBool("IsAimMode");
    }

    /// <summary> ��� üũ�� �Լ�. ��ǿ� ź�� �ִٸ� true, ������ false </summary>
    public bool ChamberIs
    {
        set => anim.SetBool("IsChamber", value);
        get => anim.GetBool("IsChamber");
    }

    /// <summary> �ִϸ��̼� ��� �Լ�(animName, Layer, norTime) </summary>
    public void Play(string stateName, int layer, float normalizedTime)
    {
        anim.Play(stateName, layer, normalizedTime);
    }
    /// <summary> ���� ������� �ִϸ��̼� Get �̸� bool�Լ� </summary>
    public bool CurrentAnimationIs(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetFloat(string paramName, float value)
    {
        anim.SetFloat(paramName, value);
    }
}
