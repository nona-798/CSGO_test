using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon;                      // ���� ������ ��µǴ� ����

    [Header("Components")]
    [SerializeField]
    private PlayerStatus status;                    // ���� �÷��̾� ����

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName;         // ���� �̸�
    [SerializeField]                                
    private Image imageWeaponIcon;                  // ���� ������
    [SerializeField]                                
    private Sprite[] spriteWeaponIcons;             // ���� ������ sprite �迭
    [SerializeField]                                
    private Vector2[] sizeWeaponIcons;              // ���� ������ UI ũ�� �迭

    [Header("Ammo Info")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;               // ����/�ִ� ź �� ��� text 

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
