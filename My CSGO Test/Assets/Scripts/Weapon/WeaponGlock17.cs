using System.Collections;
using UnityEngine;

public class WeaponGlock17 : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("SpawnPoint")]
    [SerializeField]
    private Transform bulletSpawnPos;
    [SerializeField]
    private Transform casingSpawnPos;
    [SerializeField]
    private Transform magazineSpawnPos;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip audioClipFire;
    [SerializeField]
    private AudioClip audioClipLeftReload;
    [SerializeField]
    private AudioClip audioClipOutReload;

    private CasingMemoryPool casingMemoryPool;
    private MagazineMemoryPool magazineMemoryPool;
    private ImpactMemoryPool impactMemoryPool;
    private BulletholeMemoryPool bulletholeMemoryPool;
    private Camera mainCamera;

    private void Awake()
    {
        base.Setup();

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        magazineMemoryPool = GetComponent<MagazineMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        bulletholeMemoryPool = GetComponent<BulletholeMemoryPool>();
        mainCamera = Camera.main;

        weaponSetting.currentAmmo = weaponSetting.reloadAmmo;
    }
    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);

        muzzleFlashEffect.SetActive(false);

        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }
    public override void StartWeaponAction(int type = 0)
    {
        if(type == 0 && isReload != true && isAttack != true)
        {
            OnAttack();
        }
    }
    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
    }
    public override void StartReload()
    {
        if (isReload == true) return;
        if (weaponSetting.maxAmmo <= 0) return;
        if (weaponSetting.currentAmmo >= 18) return;
        StopWeaponAction();
        StartCoroutine("OnReload");
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
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
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
            weaponSetting.reloadAmmo = 17;
        }
        else
        {
            weaponSetting.reloadAmmo = 18;
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
        if (Physics.Raycast(ray, out hit, weaponSetting.atkRange))
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
        if (Physics.Raycast(bulletSpawnPos.position, attackDir, out hit, weaponSetting.atkRange))
        {
            if (hit.transform.CompareTag("ImpactNormal"))
            {
                impactMemoryPool.SpawnImpact(hit);
            }
            else if (hit.transform.CompareTag("ImpactTarget"))
            {
                impactMemoryPool.SpawnImpact(hit);
            }
            else if (hit.transform.CompareTag("InteractionObject"))
            {
                impactMemoryPool.SpawnImpact(hit);
                hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPos.position, attackDir * weaponSetting.atkRange, Color.blue);
    }
    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }
}
