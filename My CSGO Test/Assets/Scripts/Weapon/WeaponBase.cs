using UnityEngine;

public enum WeaponType {  Main = 0, Sub, Melee, Throw }
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField]
    protected WeaponType weaponType;
    [SerializeField]
    protected WeaponSetting weaponSetting;

    protected float lastAttackTime = 0;
    protected bool isReload = false;
    protected bool isAttack = false;
    protected AudioSource audioSource;
    protected UpperAnimationController animator;

    // 외부에서 이벤트 함수 등록을 위한 public 선언 
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    // 외부 Get Property's
    public UpperAnimationController Animator => animator;
    public WeaponName WeaponName => weaponSetting.weaponName;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();
    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<UpperAnimationController>();
    }
}
