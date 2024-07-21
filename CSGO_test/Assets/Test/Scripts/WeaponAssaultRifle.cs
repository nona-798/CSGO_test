using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAssaultRifle : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject                  fireFlashEffect;               // 총구 이펙트 (on/off)

    [Header("Spawn Position")]
    [SerializeField]
    private Transform                   casingSpwanPos;                // 탄피 생성 위치
    [SerializeField]
    private Transform                   bulletSpwanPos;                // 총알 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip                   audioClipTakeOut;         // 총기 장착 사운드
    [SerializeField]
    private AudioClip                   audioClipFire;                 // 발사 사운드
    [SerializeField]
    private AudioClip                   audioClipDryFire;              // 헛발사 사운드
    [SerializeField]
    private AudioClip                   audioClipReload;               // 탄약 있을 때 재장전 사운드
    [SerializeField]
    private AudioClip                   audioClipOutReload;            // 탄약 없을 때 재장전 사운드

    [Header("Aim UI")]
    [SerializeField]
    private Image                       img_Aim;                       // default/aim 모드에 따라 aim 이미지 활성/비활성

    private bool isAimChange = false;
    private float defaultModeFOV = 60;
    private float aimModeFOV = 30;

    private CasingMemoryPool            casingMemoryPool;              // 탄피 생성 후 활성/비활성 관리
    private ImpactMemoryPool            impactMemoryPool;              // 공격 효과 생성 후 활성/ 비활성 관리
    private Camera                      mainCamera;                    // 광선 발사

    private void Awake()
    {
        // 기반 클래스의 초기화를 위한 Setup() 메소드 호출
        base.Setup();

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera       = Camera.main;

        // 처음 탄창 & 탄약 수는 최대로 설정
        weaponSetting.currentMag = weaponSetting.maxMag;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        // 무기 장착 사운드 재생
        PlaySound(audioClipTakeOut);
        // 총구 이펙트 오브젝트 비활성화
        fireFlashEffect.SetActive(false);

        // 무기가 활성화될 때 해당 무기의 탄창 & 탄약 수 정보를 갱신한다.
        onMagazineEvent.Invoke(weaponSetting.currentMag);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }
    public override void StartWeaponAction(int type = 0)
    {
        // 만약 재장전일 때 무기 액션을 할 수 없다.
        if (isReload == true) return;

        // 모드 전환 중이면 무기 액션을 할 수 없다.
        if (isAimChange == true) return;

        // 만약 공실에 탄이 없다면 무기 액션을 할 수 없다.
        //if (isEmpty == true)
        //{
        //    StartDryfire();
        //}

        // 마우스 왼쪽 클릭 (공격 시작)
        if (type == 0)
        {
            // 연발
            if (weaponSetting.isAutomaticAttack == true)
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            // 단발
            else
            {
                OnAttack();
            }
        }
        // 마우스 오른쪽 클릭(모드 전환)
        else
        {
            // 공격 중일 때는 모드 전환을 할 수 없다.
            if (isAttack == true) return;

            StartCoroutine("OnAimChange");
        }
          
    }
    public override void StopWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭 (공격 종료)
        if(type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }
    public override void StartReload()
    {
        // 현재 재장전 중이면 재장전 불가능
        if (isReload == true) return;
        if (weaponSetting.currentMag == 0) return;
        if (weaponSetting.currentAmmo >= 31) return;
        // 무기 액션 도중에 'R'키를 눌러 재장전을 시도하면 무기 액션 종료 후 재장전
        StopWeaponAction();

        StartCoroutine("OnReload");
    }
    public override void StartDryfire()
    {
        if (isAttack == true) return;
        if (isReload == true) return;
        if (isDryfire == true) return;
        StopWeaponAction();

        StartCoroutine("OnDryFire");
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

            // 탄약 수가 모자라면 공격 불가능
            if(weaponSetting.currentAmmo <= 0)
            {
                animator.ChamberIs = false;
                return;
            }

            // 공격시 currentAmmo -= 1, 탄약 수 UI 업데이트
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // 무기 애니메이션 재생 (모드에 따라 AimFir or Fire 재생)
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);
            // 총구 이펙트 재생
            if(animator.AimModeIs == false) StartCoroutine("OnFireFlashEffect");
            
            // 발사 사운드 재생
            PlaySound(audioClipFire);

            // 탄피 생성
            casingMemoryPool.SpawnCasing(casingSpwanPos.position, transform.right);

            // 광선을 발사해 원하는 위치 공격 (+Impact Effect)
            TwoStepRaycast();
        }
    }
    private IEnumerator OnFireFlashEffect()
    {
        fireFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        fireFlashEffect.SetActive(false);
    }
    private IEnumerator OnReload()
    {
        // 재장전 체크
        isReload = true;

        // 재장전 애니메이션, 사운드 재생
        animator.OnReload();

        // 남은 탄약이 1발 이상이라면 isAmmo = true
        if (weaponSetting.currentAmmo >= 1)
        {
            animator.ChamberIs = true;
        }
        else animator.ChamberIs = false;

        // animtor.Ammo 결과 값에 따라 0 or 1, audioClip도 결과 값에 따라 바꿈
        animator.Ammo    = animator.ChamberIs == true ? 1 : 0;
        audioSource.clip = animator.ChamberIs == true ? audioClipReload : audioClipOutReload;

        PlaySound(audioSource.clip);

        while (true)
        {
            // 사운드가 재생 중이 아니고, 현재 애니메이션이 Reload가 아니라면
            // 재장전 애니메이션, 사운드 재생이 종료되었다는 뜻
            if (audioSource.isPlaying == false && !animator.CurrentAnimationIs("Reload"))
            {
                isReload = false;
                // 현재 탄창 수를 1 감소시키고, 바뀐 탄창 수 정보를 Text UI에 업데이트
                weaponSetting.currentMag--;
                onMagazineEvent.Invoke(weaponSetting.currentMag);

                // 현재 탄약 수를 최대로 설정하고, 바뀐 탄약 수 정보를 Text UI에 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;

                if (animator.ChamberIs == true)
                {
                    weaponSetting.currentAmmo += 1;
                }

                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }
    private IEnumerator OnDryFire()
    {
        isDryfire = true;

        animator.OnEmpty();
        PlaySound(audioClipDryFire);
        while (true)
        {
            if (audioSource.isPlaying == false && !animator.CurrentAnimationIs("Stop"))
            {
                isDryfire = false;

                yield break;
            }
            yield return null;
        }
    }
    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면의 중앙 좌표(Aim 기준으로 Raycast 연산)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        // 공격 사거리(atkDis) 안에 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if(Physics.Raycast(ray, out hit, weaponSetting.attackDis))
        {
            targetPoint = hit.point;
        }
        // 공격 사거리 안에 부딪히는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDis;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDis, Color.red);

        // 첫번째 Raycast연산으로 얻어진 targetPoint를 목표지점으로 설정
        // 총구를 시작지점으로 하여 Raycast 연산
        Vector3 atkDir = (targetPoint - bulletSpwanPos.position).normalized;
        if(Physics.Raycast(bulletSpwanPos.position, atkDir, out hit, weaponSetting.attackDis))
        {
            impactMemoryPool.SpawnImpact(hit);
            if(hit.transform.CompareTag("ImpactEnemy"))
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(weaponSetting.damage);
            }
            else if( hit.transform.CompareTag("InteractionObject"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpwanPos.position, atkDir * weaponSetting.attackDis, Color.blue);
    }

    private IEnumerator OnAimChange()
    {
        float current = 0;
        float percent = 0;
        float time    = 0.35f;

        animator.AimModeIs = !animator.AimModeIs;
        img_Aim.enabled = !img_Aim.enabled;

        float start = mainCamera.fieldOfView;
        float end = animator.AimModeIs == true ? aimModeFOV : defaultModeFOV;
        isAimChange = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            isAimChange = true;

            // 모드에 따라 카메라 변경
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        isAimChange = false;
    }
    
    private void ResetVariables()
    {
        isReload     = false;
        isAttack     = false;
        isAimChange = false;
    }
}
