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

        // 무기가 활성화될 때 해당 무기의 탄창 정보를 갱신
        onMagazineEvent.Invoke(weaponSetting.currentMag);
        // 무기가 활성화될 때 무기의 탄 수 정보를 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    private void Awake()
    {
        base.Setup();

        // 처음 탄창 수는 최대로 설정
        weaponSetting.currentMag = weaponSetting.maxMag;
        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return;

        // 연속 공격
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

        // 공격 모션 선택
        animator.SetFloat("AttackType", type);
        // 공격 모션 재생
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
