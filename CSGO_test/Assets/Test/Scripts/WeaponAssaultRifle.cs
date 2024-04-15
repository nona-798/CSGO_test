using System.Collections;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject                  fireFlashEffect;               // 총구 이펙트 (on/off)

    [Header("Spawn Position")]
    [SerializeField]
    private Transform                   casingSpwanPos;                // 탄피 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip                   audioClipTakeOutRifle;         // 총기 장착 사운드
    [SerializeField]
    private AudioClip                   audioClipFire;                 // 발사 사운드

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting               weaponSetting;                 // 무기 설정

    private float                       lastAttackTime = 0;            // 마지막 발사시간 체크용

    private AudioSource                 audioSource;                   // 사운드 재생 컴포넌트
    private PlayerAnimationController   anim;                          // 애니메이션 재생 제어
    private CasingMemoryPool            casingMemoryPool;              // 탄피 생성 후 활성/비활성 관리

    private void Awake()
    {
        audioSource      = GetComponent<AudioSource>();
        anim             = GetComponentInParent<PlayerAnimationController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
    }
    private void OnEnable()
    {
        // 무기 장착 사운드 재생
        PlaySound(audioClipTakeOutRifle);
        // 총구 이펙트 오브젝트 비활성화
        fireFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭 (공격 시작)
        if(type == 0)
        {
            // 연발
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            // 단발
            else
            {
                OnAttack();
            }
        }
    }
    public void StopWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭 (공격 종료)
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    private IEnumerator OnAttackLoop()
    {
        while(true)
        {
            OnAttack();

            yield return null;
        }
    }

    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            //움직이면 총알이 크게 튀게끔 만들어야 함
            // if(anim.MoveSpeed <= 0.5f)

            // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재시간 저장
            lastAttackTime = Time.time;
            // 무기 애니메이션 재생
            anim.Play("Fire", -1, 0);
            // 총구 이펙트 재생
            StartCoroutine("OnFireFlashEffect");
            // 발사 사운드 재생
            PlaySound(audioClipFire);
            // 탄피 생성
            casingMemoryPool.SpawnCasing(casingSpwanPos.position, transform.right);
        }
    }
    private IEnumerator OnFireFlashEffect()
    {
        fireFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        fireFlashEffect.SetActive(false);
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // 기존에 재생하던 사운드 정지
        audioSource.clip = clip;    // 새로운 사운드 clip으로 교체
        audioSource.Play();         // 교체된 clip을 재생
    }
}
