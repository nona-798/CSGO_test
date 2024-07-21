using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [SerializeField]
    private WeaponKnifeCollider knifeCollider;

    private void OnEnable()
    {
        isAttack = false;

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ ������ ����
        onMagazineEvent.Invoke(weaponSetting.currentMag);
        // ���Ⱑ Ȱ��ȭ�� �� ������ ź �� ������ ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    private void Awake()
    {
        base.Setup();

        // ó�� źâ ���� �ִ�� ����
        weaponSetting.currentMag = weaponSetting.maxMag;
        // ó�� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return;

        // ���� ����
        if(weaponSetting.isAutomaticAttack == true)
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else
        {
            StartCoroutine("OnAttack", type);
        }
    }
    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
        StopCoroutine("OnAttackLoop");
    }
    public override void StartReload()
    {
        
    }
    private IEnumerator OnAttackLoop(int type)
    {
        while(true)
        {
            yield return StartCoroutine("OnAttack", type); 
        }
    }
    private IEnumerator OnAttack(int type)
    {
        isAttack = true;

        // ���� ��� ����
        animator.SetFloat("AttackType", type);
        // ���� ��� ���
        animator.Play("Fire", -1, 0);
        yield return new WaitForEndOfFrame();

        while(true)
        {
            if(animator.CurrentAnimationIs("Movement"))
            {
                isAttack = false;

                yield break;
            }
            yield return null;
        }
    }
    public void StartWeaponKnifeCollider()
    {
        knifeCollider.StartCollider(weaponSetting.damage);
    }
    public override void StartDryfire()
    {
        return;
    }
}
