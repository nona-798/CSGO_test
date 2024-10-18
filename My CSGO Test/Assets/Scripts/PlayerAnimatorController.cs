using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsStand", true);
    }
    public float MoveSpeed
    {
        set => animator.SetFloat("MoveSpeed", value);
        get => animator.GetFloat("MoveSpeed");
    }
    public float DirX
    {
        set => animator.SetFloat("DirX", value);
        get => animator.GetFloat("DirX");
    }
    public float DirZ
    {
        set => animator.SetFloat("DirZ", value);
        get => animator.GetFloat("DirZ");
    }

    public void IsStand(bool value)
    {
        animator.SetBool("IsStand", value);
    }
    public bool GetStand()
    {
        return animator.GetBool("IsStand");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }
    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
