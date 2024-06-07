using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazinEvent : UnityEngine.Events.UnityEvent<int> { }
public class WeaponAssaultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent                    onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazinEvent                 onMagazineEvent = new MagazinEvent();

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
    private AudioClip                   audioClipTakeOutRifle;         // �ѱ� ���� ����
    [SerializeField]
    private AudioClip                   audioClipFire;                 // �߻� ����
    [SerializeField]
    private AudioClip                   audioClipReload;               // ź�� ���� �� ������ ����
    [SerializeField]
    private AudioClip                   audioClipOutReload;            // ź�� ���� �� ������ ����

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting               weaponSetting;                 // ���� ����

    [Header("Weapon Setting")]
    [SerializeField]
    private Image                       img_Aim;                       // default/aim ��忡 ���� aim �̹��� Ȱ��/��Ȱ��

    private float                       lastAttackTime = 0;            // ������ �߻�ð� üũ��
    private bool                        isReload = false;              // ������ ����
    private bool isAttack = false;
    private bool isModeChange = false;
    private float defaultModeFOV = 60;
    private float aimModeFOV = 30;

    private AudioSource                 audioSource;                   // ���� ��� ������Ʈ
    private PlayerAnimationController   anim;                          // �ִϸ��̼� ��� ����
    private CasingMemoryPool            casingMemoryPool;              // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private ImpactMemoryPool            impactMemoryPool;              // ���� ȿ�� ���� �� Ȱ��/ ��Ȱ�� ����
    private Camera                      mainCamera;                    // ���� �߻�
    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ Get ������Ƽ
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int        CurrentMagazine => weaponSetting.currentMag;
    public int        MaxMagazine => weaponSetting.maxMag;
    private void Awake()
    {
        audioSource      = GetComponent<AudioSource>();
        anim             = GetComponentInParent<PlayerAnimationController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera       = Camera.main;
        // ó�� źâ ���� �ִ�� ����
        weaponSetting.currentMag = weaponSetting.maxMag;
        // ó�� ź�� ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        // ���� ���� ���� ���
        PlaySound(audioClipTakeOutRifle);
        // �ѱ� ����Ʈ ������Ʈ ��Ȱ��ȭ
        fireFlashEffect.SetActive(false);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ �� ������ �����Ѵ�.
        onMagazineEvent.Invoke(weaponSetting.currentMag);
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź�� �� ������ �����Ѵ�.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }
    public void StartWeaponAction(int type = 0)
    {
        // ���� �������� �� ���� �׼��� �� �� ����.
        if (isReload == true) return;

        // ��� ��ȯ ���̸� ���� �׼��� �� �� ����.
        if (isModeChange == true) return;

        // ���콺 ���� Ŭ�� (���� ����)
        if (type == 0)
        {
            // ����
            if(weaponSetting.isAutomaticAttack == true)
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

            StartCoroutine("OnModeChange");
        }    
    }
    public void StopWeaponAction(int type = 0)
    {
        // ���콺 ���� Ŭ�� (���� ����)
        if(type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }
    public void StartReload()
    {
        // ���� ������ ���̸� ������ �Ұ���
        if (isReload == true) return;
        if (weaponSetting.currentMag == 0) return;
        if (weaponSetting.currentAmmo >= 31) return;
        // ���� �׼� ���߿� 'R'Ű�� ���� �������� �õ��ϸ� ���� �׼� ���� �� ������
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
                return;
            }

            // ���ݽ� currentAmmo -= 1, ź�� �� UI ������Ʈ
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ��� (��忡 ���� AimFir or Fire ���)
            string animation = anim.AimModeIs == true ? "AimFire" : "Fire";
            anim.Play(animation, -1, 0);
            // �ѱ� ����Ʈ ���
            if(anim.AimModeIs == false) StartCoroutine("OnFireFlashEffect");
            
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
        isReload = true;
        bool isAmmo = false;
        // ������ �ִϸ��̼�, ���� ���
        anim.OnReload();

        // ���� ź���� 1�� �̻��̶�� isAmmo = true
        if (weaponSetting.currentAmmo >= 1)
        {
            isAmmo = true;
        }
        
        // anim.Ammo ��� ���� ���� 0 or 1, audioClip�� ��� ���� ���� �ٲ�
        anim.Ammo =        isAmmo == true ? 1.0f            : 0.0f;
        audioSource.clip = isAmmo == true ? audioClipReload : audioClipOutReload;

        audioSource.Play();

        while(true)
        {
            // ���尡 ��� ���� �ƴϰ�, ���� �ִϸ��̼��� Reload�� �ƴ϶��
            // ������ �ִϸ��̼�, ���� ����� ����Ǿ��ٴ� ��
            if (audioSource.isPlaying == false && !anim.CurrentAnimationIs("Reload"))
            {
                isReload = false;
                
                // ���� źâ ���� 1 ���ҽ�Ű��, �ٲ� źâ �� ������ Text UI�� ������Ʈ
                weaponSetting.currentMag--;
                onMagazineEvent.Invoke(weaponSetting.currentMag);
                // ���� ź�� ���� �ִ�� �����ϰ�, �ٲ� ź�� �� ������ Text UI�� ������Ʈ
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                if (isAmmo == true)
                {
                    weaponSetting.currentAmmo = weaponSetting.maxAmmo + 1;
                }
                isAmmo = false;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

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
        }
        Debug.DrawRay(bulletSpwanPos.position, atkDir * weaponSetting.attackDis, Color.blue);
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // ������ ����ϴ� ���� ����
        audioSource.clip = clip;    // ���ο� ���� clip���� ��ü
        audioSource.Play();         // ��ü�� clip�� ���
    }
    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time    = 0.35f;

        anim.AimModeIs = !anim.AimModeIs;
        img_Aim.enabled = !img_Aim.enabled;

        float start = mainCamera.fieldOfView;
        float end = anim.AimModeIs == true ? aimModeFOV : defaultModeFOV;
        isModeChange = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            isModeChange = true;

            // ��忡 ���� ī�޶� ����
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        isModeChange = false;
    }
    
    private void ResetVariables()
    {
        isReload     = false;
        isAttack     = false;
        isModeChange = false;
    }
}
