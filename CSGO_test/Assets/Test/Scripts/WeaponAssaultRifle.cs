using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAssaultRifle : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject                  fireFlashEffect;               // �ѱ� ����Ʈ (on/off)

    [Header("Spawn Position")]
    [SerializeField]
    private Transform                   casingSpwanPos;                // ź�� ���� ��ġ
    [SerializeField]
    private Transform                   bulletSpwanPos;                // �Ѿ� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip                   audioClipTakeOut;         // �ѱ� ���� ����
    [SerializeField]
    private AudioClip                   audioClipFire;                 // �߻� ����
    [SerializeField]
    private AudioClip                   audioClipDryFire;              // ��߻� ����
    [SerializeField]
    private AudioClip                   audioClipReload;               // ź�� ���� �� ������ ����
    [SerializeField]
    private AudioClip                   audioClipOutReload;            // ź�� ���� �� ������ ����

    [Header("Aim UI")]
    [SerializeField]
    private Image                       img_Aim;                       // default/aim ��忡 ���� aim �̹��� Ȱ��/��Ȱ��

    private bool isAimChange = false;
    private float defaultModeFOV = 60;
    private float aimModeFOV = 30;

    private CasingMemoryPool            casingMemoryPool;              // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private ImpactMemoryPool            impactMemoryPool;              // ���� ȿ�� ���� �� Ȱ��/ ��Ȱ�� ����
    private Camera                      mainCamera;                    // ���� �߻�

    private void Awake()
    {
        // ��� Ŭ������ �ʱ�ȭ�� ���� Setup() �޼ҵ� ȣ��
        base.Setup();

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera       = Camera.main;

        // ó�� źâ & ź�� ���� �ִ�� ����
        weaponSetting.currentMag = weaponSetting.maxMag;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        // ���� ���� ���� ���
        PlaySound(audioClipTakeOut);
        // �ѱ� ����Ʈ ������Ʈ ��Ȱ��ȭ
        fireFlashEffect.SetActive(false);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ & ź�� �� ������ �����Ѵ�.
        onMagazineEvent.Invoke(weaponSetting.currentMag);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }
    public override void StartWeaponAction(int type = 0)
    {
        // ���� �������� �� ���� �׼��� �� �� ����.
        if (isReload == true) return;

        // ��� ��ȯ ���̸� ���� �׼��� �� �� ����.
        if (isAimChange == true) return;

        // ���� ���ǿ� ź�� ���ٸ� ���� �׼��� �� �� ����.
        //if (isEmpty == true)
        //{
        //    StartDryfire();
        //}

        // ���콺 ���� Ŭ�� (���� ����)
        if (type == 0)
        {
            // ����
            if (weaponSetting.isAutomaticAttack == true)
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            // �ܹ�
            else
            {
                OnAttack();
            }
        }
        // ���콺 ������ Ŭ��(��� ��ȯ)
        else
        {
            // ���� ���� ���� ��� ��ȯ�� �� �� ����.
            if (isAttack == true) return;

            StartCoroutine("OnAimChange");
        }
          
    }
    public override void StopWeaponAction(int type = 0)
    {
        // ���콺 ���� Ŭ�� (���� ����)
        if(type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }
    public override void StartReload()
    {
        // ���� ������ ���̸� ������ �Ұ���
        if (isReload == true) return;
        if (weaponSetting.currentMag == 0) return;
        if (weaponSetting.currentAmmo >= 31) return;
        // ���� �׼� ���߿� 'R'Ű�� ���� �������� �õ��ϸ� ���� �׼� ���� �� ������
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
            //�����̸� �Ѿ��� ũ�� Ƣ�Բ� ������ ��
            // if(anim.MoveSpeed <= 0.5f)

            // ���� �ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ����ð� ����
            lastAttackTime = Time.time;

            // ź�� ���� ���ڶ�� ���� �Ұ���
            if(weaponSetting.currentAmmo <= 0)
            {
                animator.ChamberIs = false;
                return;
            }

            // ���ݽ� currentAmmo -= 1, ź�� �� UI ������Ʈ
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ��� (��忡 ���� AimFir or Fire ���)
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);
            // �ѱ� ����Ʈ ���
            if(animator.AimModeIs == false) StartCoroutine("OnFireFlashEffect");
            
            // �߻� ���� ���
            PlaySound(audioClipFire);

            // ź�� ����
            casingMemoryPool.SpawnCasing(casingSpwanPos.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ���� (+Impact Effect)
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
        // ������ üũ
        isReload = true;

        // ������ �ִϸ��̼�, ���� ���
        animator.OnReload();

        // ���� ź���� 1�� �̻��̶�� isAmmo = true
        if (weaponSetting.currentAmmo >= 1)
        {
            animator.ChamberIs = true;
        }
        else animator.ChamberIs = false;

        // animtor.Ammo ��� ���� ���� 0 or 1, audioClip�� ��� ���� ���� �ٲ�
        animator.Ammo    = animator.ChamberIs == true ? 1 : 0;
        audioSource.clip = animator.ChamberIs == true ? audioClipReload : audioClipOutReload;

        PlaySound(audioSource.clip);

        while (true)
        {
            // ���尡 ��� ���� �ƴϰ�, ���� �ִϸ��̼��� Reload�� �ƴ϶��
            // ������ �ִϸ��̼�, ���� ����� ����Ǿ��ٴ� ��
            if (audioSource.isPlaying == false && !animator.CurrentAnimationIs("Reload"))
            {
                isReload = false;
                // ���� źâ ���� 1 ���ҽ�Ű��, �ٲ� źâ �� ������ Text UI�� ������Ʈ
                weaponSetting.currentMag--;
                onMagazineEvent.Invoke(weaponSetting.currentMag);

                // ���� ź�� ���� �ִ�� �����ϰ�, �ٲ� ź�� �� ������ Text UI�� ������Ʈ
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

        // ȭ���� �߾� ��ǥ(Aim �������� Raycast ����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        // ���� ��Ÿ�(atkDis) �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if(Physics.Raycast(ray, out hit, weaponSetting.attackDis))
        {
            targetPoint = hit.point;
        }
        // ���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� �ִ� ��Ÿ� ��ġ
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDis;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDis, Color.red);

        // ù��° Raycast�������� ����� targetPoint�� ��ǥ�������� ����
        // �ѱ��� ������������ �Ͽ� Raycast ����
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

            // ��忡 ���� ī�޶� ����
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
