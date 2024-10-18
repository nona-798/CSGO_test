using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    private Animator anim;
    private Transform spine;
    private Transform hips;

    private float playerSpineRot;
    public float PlayerSpineRot
    {
        set => playerSpineRot = Mathf.Max(0, value);
        get => playerSpineRot;
    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spine = anim.GetBoneTransform(HumanBodyBones.Spine);
        hips = anim.GetBoneTransform(HumanBodyBones.Hips);
    }
    public void BodyXRotate(GameObject target)
    {
        hips.LookAt(target.transform.position);
        spine.LookAt(target.transform.position);
        spine.localRotation = Quaternion.Euler(playerSpineRot, 0, 0);
    }
    public void OnReload()
    {

    }
    public void OnMove()
    {
        anim.SetTrigger("IsMove");
    }
    public void OnIdle()
    {
        anim.SetTrigger("Idle");
    }
    /// <summary> Movement �б��� ���� </summary>
    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }
    /// <summary> MoveForward �б��� ���� </summary>
    public float MoveForward
    {
        set => anim.SetFloat("MoveForward", value);
        get => anim.GetFloat("MoveForward");
    }
    /// <summary> MoveRight �б��� ���� </summary>
    public float MoveStrafe
    {
        set => anim.SetFloat("MoveStrafe", value);
        get => anim.GetFloat("MoveStrafe");
    }
    public float PlayerXRotate
    {
        set => playerSpineRot = Mathf.Max(0, value);
        get => playerSpineRot;
    }
    
    /// <summary> �ִϸ��̼� ��� �Լ�(animName, Layer, norTime) </summary>
    public void Play(string stateName, int layer, float norTime)
    {
        anim.Play(stateName, layer, norTime);
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
