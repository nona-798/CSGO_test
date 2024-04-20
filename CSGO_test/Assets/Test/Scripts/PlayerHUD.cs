using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssaultRifle  weapon;                 // ���� ������ ��µǴ� ����

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI     txtWeaponName;          // ���� �̸�
    [SerializeField]
    private Image               imgWeaponIcon;          // ���� ������
    [SerializeField]
    private Sprite[]            spriteWeaponIcons;      // ���� �����ܿ� ���� Sprite �迭

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI     txtAmmo;                // (���� / �ִ�) ź�� �� ���

    [Header("Magazine")]
    [SerializeField]
    private GameObject magazineUIPrefab;                 // źâ UI ������
    [SerializeField]
    private Transform magazineParent;                    // źâ UI�� ��ġ�Ǵ� Panel

    private List<GameObject> magazineList;              // źâ UI ����Ʈ

    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();

        // �޼ҵ尡 ��ϵǾ� �ִ� �̺�Ʈ Ŭ����(weapon.xx)��
        // Invoke() �޼ҵ尡 ȣ��� �� ��ϵ� �޼ҵ�(�Ű�����)�� �����
        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
        weapon.onMagazineEvent.AddListener(UpdateMagazineHUD);
    }
    private void SetupWeapon()
    {
        txtWeaponName.text = weapon.WeaponName.ToString();
        imgWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    }
    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        txtAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }
    private void SetupMagazine()
    {
        // Weapon�� ��ϵǾ� �ִ� �ִ� źâ ������ŭ Image Icon ����
        // magazineParent ������Ʈ�� �ڽ����� ��� ��  ��� ��Ȱ��ȭ/ ����Ʈ�� ����
        magazineList = new List<GameObject>();
        for (int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        // weapon�� ��ϵǾ� �ִ� ���� źâ ������ŭ ������Ʈ Ȱ��ȭ
        for(int i = 0; i < weapon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }
    private void UpdateMagazineHUD(int curMag)
    {
        // ���� ��Ȱ��ȭ�ϰ�, currentMagazine ������ŭ Ȱ��ȭ
        for(int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < curMag; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }
}
