using System.Collections;
using UnityEngine;
public class WeaponAK47 : WeaponBase
{
    //[HideInInspector]
    //public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("FireFlash Effect")]
    [SerializeField]
    private GameObject muzzleFlashEffect;               // 총구 이펙트

    [Header("Spawn Point")]
    [SerializeField]
    private Transform casingSpawnPos;                 // 탄피 생성
    [SerializeField]
    private Transform magazineSpawnPos;               // 버린 탄창 생성
    [SerializeField]
    private Transform bulletSpawnPos;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;         // 무기 장착 사운드
    [SerializeField]
    private AudioClip audioClipFire;                  // 무기 발사 사운드
    [SerializeField]
    private AudioClip audioClipLeftReload;            // 무기 재장전 사운드 (남은 탄약 있음)
    [SerializeField]
    private AudioClip audioClipOutReload;             // 무기 재장전 사운드 (남은 탄약 없음)

    private CasingMemoryPool    casingMemoryPool;        // 탄피 생성 후 활성/비활성 관리
    private MagazineMemoryPool  magazineMemoryPool;      // 재장전 시 플레이어 위치에 빈 탄창 생성
    private ImpactMemoryPool    impactMemoryPool;
    private BulletholeMemoryPool bulletholeMemoryPool;
    private Camera              mainCamera;
    private void Awake()
    {
        // 기반 클래스의 초기화를 위한 Setup() 함수 호출
        base.Setup();

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        magazineMemoryPool = GetComponent<MagazineMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        bulletholeMemoryPool = GetComponent<BulletholeMemoryPool>();
        mainCamera = Camera.main;

        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.reloadAmmo;
    }
    
    private void OnEnable()
    {
        // 무기 장착 사운드
        PlaySound(audioClipTakeOutWeapon);

        // 총구 이펙트 비활성
        muzzleFlashEffect.SetActive(false);

        // 무기 장착하면 해당 무기의 탄 수 정보를 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    /// <summary> 총기 액션 시작 함수 </summary>
    public override void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return;

        // 마우스 오른쪽 클릭
        if(type == 0)
        {
            // 연발
            if (weaponSetting.IsAutomatic == true)
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
    /// <summary> 총기 액션 정지 함수 </summary>
    public override void StopWeaponAction(int type = 0)
    {
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    public override void StartReload()
    {
        if (isReload == true) return;
        if (weaponSetting.maxAmmo <= 0) return;
        if (weaponSetting.currentAmmo >= 31) return;
        StopWeaponAction();
        StartCoroutine("OnReload");
    }
    private IEnumerator OnAttackLoop()
    {
        while(true)
        {
            OnAttack();

            yield return null;
        }
    }
    private void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.atkRate)
        {
            //RaycastHit hit;
            // 마지막 공격 주기 저장
            lastAttackTime = Time.time;

            // 탄이 없으면 발사 불가능
            if (weaponSetting.currentAmmo <= 0) return;

            // 발사하면 현재 탄 수 감소시키고 탄 수 UI 업데이트
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // 발사 애니메이션 재생
            animator.Play("Fire", -1, 0);

            // 총구 화염 애니메이션 재생
            StartCoroutine("OnFireFlashEffect");
            //if(Physics.Raycast(mainCamera.transform.position,mainCamera.transform.forward, out hit,weaponSetting.atkRange))
            //{
            //    bulletholeMemoryPool.SpawnBullethole(hit);
            //}
            // 발사 사운드 재생
            PlaySound(audioClipFire);

            // 탄피 생성
            casingMemoryPool.SpawnCasing(casingSpawnPos.position, transform.right);

            // 광선 발사해 원하는 위치 공격 + impact effect
            TwoStepRayCast();
        }
    }
    private IEnumerator OnReload()
    {
        // 재장전 여부 체크
        isReload = true;

        // 재장전 사운드, 애니메이션 재생
        if (weaponSetting.currentAmmo <= 0)
        {
            animator.LeftAmmo = 0.0f;
            animator.OnReload();
            PlaySound(audioClipOutReload);
        }
        else
        {
            animator.LeftAmmo = 1.0f;
            animator.OnReload();
            PlaySound(audioClipLeftReload);
        }

        // 버려진 탄창 생성
        magazineMemoryPool.SpawnMagazine(magazineSpawnPos.position, -transform.up);
        

        while (true)
        {
            // 사운드 재생 과 애니메이션이 끝나면 대기 모션으로 돌아감
            if (audioSource.isPlaying==false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                CheckAmmo();

                yield break;
            }

            yield return null;
        }
    }
    /// <summary> 재장전시 탄약 조건식 함수</summary>
    private void CheckAmmo()
    {
        // 현재 탄약과 남은 탄약을 합침
        weaponSetting.maxAmmo = weaponSetting.maxAmmo + weaponSetting.currentAmmo;

        // 약실의 탄 존재 여부
        if (animator.LeftAmmo == 0.0f)
        {
            weaponSetting.reloadAmmo = 30;
        }
        else
        {
            weaponSetting.reloadAmmo = 31;
        }

        // LeftAmmo < 30
        if (weaponSetting.maxAmmo < weaponSetting.reloadAmmo)
        {
            weaponSetting.reloadAmmo = weaponSetting.maxAmmo;
        }

        weaponSetting.currentAmmo = weaponSetting.reloadAmmo;
        weaponSetting.maxAmmo -= weaponSetting.reloadAmmo;
        

        // 탄약 수 정보 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    private IEnumerator OnFireFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.atkRate * 0.2f);
        muzzleFlashEffect.SetActive(false);
    }
    private void TwoStepRayCast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면 중앙 좌표 (Aim 기준으로 RayCast 연산)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // 공격 사거리(atkRange) 안에 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if(Physics.Raycast(ray, out hit, weaponSetting.atkRange))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.atkRange;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.atkRange, Color.red);
        // 첫번째 Raycast연산으로 얻어진 targetPoint를 목표지점으로 설정하고,
        // 총구를 시작 지점으로 시작하여 Raycast연산
        Vector3 attackDir = (targetPoint - bulletSpawnPos.position).normalized;
        if(Physics.Raycast(bulletSpawnPos.position,attackDir,out hit, weaponSetting.atkRange))
        {
            if(hit.transform.CompareTag("ImpactNormal"))
            {
                impactMemoryPool.SpawnImpact(hit);
            }
            else if (hit.transform.CompareTag("ImpactTarget"))
            {
                impactMemoryPool.SpawnImpact(hit);
            }
            else if(hit.transform.CompareTag("InteractionObject"))
            {
                impactMemoryPool.SpawnImpact(hit);
                hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPos.position, attackDir * weaponSetting.atkRange, Color.blue);
    }
}
