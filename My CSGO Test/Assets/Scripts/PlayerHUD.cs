using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon;                      // 현재 정보가 출력되는 무기

    [Header("Components")]
    [SerializeField]
    private PlayerStatus status;                    // 현재 플레이어 상태

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName;         // 무기 이름
    [SerializeField]                                
    private Image imageWeaponIcon;                  // 무기 아이콘
    [SerializeField]                                
    private Sprite[] spriteWeaponIcons;             // 무기 아이콘 sprite 배열
    [SerializeField]                                
    private Vector2[] sizeWeaponIcons;              // 무기 아이콘 UI 크기 배열

    [Header("Ammo Info")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;               // 현재/최대 탄 수 출력 text 

    [Header("HP & AP")]
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private TextMeshProUGUI textAP;
    [SerializeField]
    private Image imgBloodScreen;
    [SerializeField]
    private AnimationCurve curveBloodScreen;

    [Header("Flash")]
    [SerializeField]
    private Image imgFlashScreen;
    [SerializeField]
    private AnimationCurve curveFlashScreen;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
        status.onAPEvent.AddListener(UpdateAPHUD);
    }
    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        for(int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
        }
    }
    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
        SetupWeapon();
    }
    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    }
    private void UpdateAmmoHUD(int curAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{curAmmo}/</size>{maxAmmo}";
    }
    private void UpdateHPHUD(int previous, int current)
    {
        textHP.text = current.ToString();

        if(previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }
    private void UpdateAPHUD(int previous, int current)
    {
        textAP.text = current.ToString();
    }
    private void UpdateFlashHUD()
    {

    }
    private IEnumerator OnBloodScreen()
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imgBloodScreen.color;
            color.a = Mathf.Lerp(0.75f, 0, curveBloodScreen.Evaluate(percent));
            imgBloodScreen.color = color;

            yield return null;
        }
    }
}
