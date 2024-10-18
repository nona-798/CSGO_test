using System.Collections;
using UnityEngine;
public class WeaponAK47 : WeaponBase
{
    //[HideInInspector]
    //public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("FireFlash Effect")]
    [SerializeField]
    private GameObject muzzleFlashEffect;               // �ѱ� ����Ʈ

    [Header("Spawn Point")]
    [SerializeField]
    private Transform casingSpawnPos;                 // ź�� ����
    [SerializeField]
    private Transform magazineSpawnPos;               // ���� źâ ����
    [SerializeField]
    private Transform bulletSpawnPos;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;         // ���� ���� ����
    [SerializeField]
    private AudioClip audioClipFire;                  // ���� �߻� ����
    [SerializeField]
    private AudioClip audioClipLeftReload;            // ���� ������ ���� (���� ź�� ����)
    [SerializeField]
    private AudioClip audioClipOutReload;             // ���� ������ ���� (���� ź�� ����)

    private CasingMemoryPool    casingMemoryPool;        // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private MagazineMemoryPool  magazineMemoryPool;      // ������ �� �÷��̾� ��ġ�� �� źâ ����
    private ImpactMemoryPool    impactMemoryPool;
    private BulletholeMemoryPool bulletholeMemoryPool;
    private Camera              mainCamera;
    private void Awake()
    {
        // ��� Ŭ������ �ʱ�ȭ�� ���� Setup() �Լ� ȣ��
        base.Setup();

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        magazineMemoryPool = GetComponent<MagazineMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        bulletholeMemoryPool = GetComponent<BulletholeMemoryPool>();
        mainCamera = Camera.main;

        // ó�� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.reloadAmmo;
    }
    
    private void OnEnable()
    {
        // ���� ���� ����
        PlaySound(audioClipTakeOutWeapon);

        // �ѱ� ����Ʈ ��Ȱ��
        muzzleFlashEffect.SetActive(false);

        // ���� �����ϸ� �ش� ������ ź �� ������ ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    /// <summary> �ѱ� �׼� ���� �Լ� </summary>
    public override void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return;

        // ���콺 ������ Ŭ��
        if(type == 0)
        {
            // ����
            if (weaponSetting.IsAutomatic == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            // �ܹ�
            else
            {
                OnAttack();
            }
        }
    }
    /// <summary> �ѱ� �׼� ���� �Լ� </summary>
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
            // ������ ���� �ֱ� ����
            lastAttackTime = Time.time;

            // ź�� ������ �߻� �Ұ���
            if (weaponSetting.currentAmmo <= 0) return;

            // �߻��ϸ� ���� ź �� ���ҽ�Ű�� ź �� UI ������Ʈ
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // �߻� �ִϸ��̼� ���
            animator.Play("Fire", -1, 0);

            // �ѱ� ȭ�� �ִϸ��̼� ���
            StartCoroutine("OnFireFlashEffect");
            //if(Physics.Raycast(mainCamera.transform.position,mainCamera.transform.forward, out hit,weaponSetting.atkRange))
            //{
            //    bulletholeMemoryPool.SpawnBullethole(hit);
            //}
            // �߻� ���� ���
            PlaySound(audioClipFire);

            // ź�� ����
            casingMemoryPool.SpawnCasing(casingSpawnPos.position, transform.right);

            // ���� �߻��� ���ϴ� ��ġ ���� + impact effect
            TwoStepRayCast();
        }
    }
    private IEnumerator OnReload()
    {
        // ������ ���� üũ
        isReload = true;

        // ������ ����, �ִϸ��̼� ���
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

        // ������ źâ ����
        magazineMemoryPool.SpawnMagazine(magazineSpawnPos.position, -transform.up);
        

        while (true)
        {
            // ���� ��� �� �ִϸ��̼��� ������ ��� ������� ���ư�
            if (audioSource.isPlaying==false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                CheckAmmo();

                yield break;
            }

            yield return null;
        }
    }
    /// <summary> �������� ź�� ���ǽ� �Լ�</summary>
    private void CheckAmmo()
    {
        // ���� ź��� ���� ź���� ��ħ
        weaponSetting.maxAmmo = weaponSetting.maxAmmo + weaponSetting.currentAmmo;

        // ����� ź ���� ����
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
        

        // ź�� �� ���� ����
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

        // ȭ�� �߾� ��ǥ (Aim �������� RayCast ����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // ���� ��Ÿ�(atkRange) �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if(Physics.Raycast(ray, out hit, weaponSetting.atkRange))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.atkRange;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.atkRange, Color.red);
        // ù��° Raycast�������� ����� targetPoint�� ��ǥ�������� �����ϰ�,
        // �ѱ��� ���� �������� �����Ͽ� Raycast����
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
