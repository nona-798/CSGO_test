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

    /// <summary> Reload 트리거 함수 </summary>
    public void OnReload()
    {
        anim.SetTrigger("OnReload");
    }

    ///  <summary> Empty 트리거 함수 </summary>
    public void OnEmpty()
    {
        anim.SetTrigger("OnEmpty");
    }

    /// <summary> Movement 분기점 변수 </summary>
    public float MoveSpeed
    {
        set => anim.SetFloat("MoveSpeed", value);
        get => anim.GetFloat("MoveSpeed");
    }

    /// <summary> Reload 분기점 변수 </summary>
    public float Ammo
    {
        set => anim.SetFloat("Ammo", value);
        get => anim.GetFloat("Ammo");
    }

    /// <summary> Assault Rifle 마우스 오른쪽 클릭 액션 (default/ aim mode) </summary>
    public bool AimModeIs
    {
        set => anim.SetBool("IsAimMode", value);
        get => anim.GetBool("IsAimMode");
    }

    /// <summary> 약실 체크용 함수. 약실에 탄이 있다면 true, 없으면 false </summary>
    public bool ChamberIs
    {
        set => anim.SetBool("IsChamber", value);
        get => anim.GetBool("IsChamber");
    }

    /// <summary> 애니메이션 재생 함수(animName, Layer, norTime) </summary>
    public void Play(string stateName, int layer, float normalizedTime)
    {
        anim.Play(stateName, layer, normalizedTime);
    }
    /// <summary> 현재 재생중인 애니메이션 Get 이름 bool함수 </summary>
    public bool CurrentAnimationIs(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetFloat(string paramName, float value)
    {
        anim.SetFloat(paramName, value);
    }
}
