using System.Collections;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject                  fireFlashEffect;               // �ѱ� ����Ʈ (on/off)

    [Header("Spawn Position")]
    [SerializeField]
    private Transform                   casingSpwanPos;                // ź�� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip                   audioClipTakeOutRifle;         // �ѱ� ���� ����
    [SerializeField]
    private AudioClip                   audioClipFire;                 // �߻� ����

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting               weaponSetting;                 // ���� ����

    private float                       lastAttackTime = 0;            // ������ �߻�ð� üũ��

    private AudioSource                 audioSource;                   // ���� ��� ������Ʈ
    private PlayerAnimationController   anim;                          // �ִϸ��̼� ��� ����
    private CasingMemoryPool            casingMemoryPool;              // ź�� ���� �� Ȱ��/��Ȱ�� ����

    private void Awake()
    {
        audioSource      = GetComponent<AudioSource>();
        anim             = GetComponentInParent<PlayerAnimationController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
    }
    private void OnEnable()
    {
        // ���� ���� ���� ���
        PlaySound(audioClipTakeOutRifle);
        // �ѱ� ����Ʈ ������Ʈ ��Ȱ��ȭ
        fireFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type = 0)
    {
        // ���콺 ���� Ŭ�� (���� ����)
        if(type == 0)
        {
            // ����
            if(weaponSetting.isAutomaticAttack == true)
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
    public void StopWeaponAction(int type = 0)
    {
        // ���콺 ���� Ŭ�� (���� ����)
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
            //�����̸� �Ѿ��� ũ�� Ƣ�Բ� ������ ��
            // if(anim.MoveSpeed <= 0.5f)

            // ���� �ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ����ð� ����
            lastAttackTime = Time.time;
            // ���� �ִϸ��̼� ���
            anim.Play("Fire", -1, 0);
            // �ѱ� ����Ʈ ���
            StartCoroutine("OnFireFlashEffect");
            // �߻� ���� ���
            PlaySound(audioClipFire);
            // ź�� ����
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
        audioSource.Stop();         // ������ ����ϴ� ���� ����
        audioSource.clip = clip;    // ���ο� ���� clip���� ��ü
        audioSource.Play();         // ��ü�� clip�� ���
    }
}
