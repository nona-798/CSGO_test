using UnityEngine;

public enum WeaponType { Main = 0, Sub, Melee, Throw }
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }
public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField]
    protected WeaponType                weaponType;                // 무기 종류
    [SerializeField]                                               
    protected WeaponSetting             weaponSetting;             // 무기 설정
                                                                   
    protected float                     lastAttackTime = 0;        // 마지막 발사시간 체크용
    protected bool                      isReload = false;          // 재장전 중인지 체크
    protected bool                      isEmpty = false;           // 약실에 탄이 있는지 체크
    protected bool                      isDryfire = false;         // 헛발사 중인지 체크
    protected bool                      isAttack = false;          // 공격 여부  체크용
    protected AudioSource               audioSource;               // 사운드 재생 컴포넌트
    protected PlayerAnimationController animator;                  // 애니메이션 재생 제어

    // 외부에서 이벤트 함수 등록을 할 수 있도록 public 선언
    [HideInInspector]
    public AmmoEvent                     onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent                 onMagazineEvent = new MagazineEvent();

    // 외부에서 필요한 정보를 열람하기 위해 정의한 Get property's
    public PlayerAnimationController    Animator => animator;
    public WeaponName                   WeaponName => weaponSetting.weaponName;
    public int                          CurrentMagazine => weaponSetting.currentMag;
    public int                          MaxMagazine => weaponSetting.maxMag;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();
    public abstract void StartDryfire();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator    = GetComponent<PlayerAnimationController>();
    }
}
