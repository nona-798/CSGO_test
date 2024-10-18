using UnityEngine;

public class UpperAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public float MoveSpeed
    {
        set => animator.SetFloat("MoveSpeed", value);
        get => animator.GetFloat("MoveSpeed");
    }
    public float LeftAmmo
    {
        set => animator.SetFloat("AmmoLeft", value);
        get => animator.GetFloat("AmmoLeft");
    }
    public void OnReload()
    {
        animator.SetTrigger("OnReload");
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
