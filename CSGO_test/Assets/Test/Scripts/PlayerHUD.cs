using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase          weapon;                 // ���� ������ ��µǴ� ����

    [Header("Components")]
    [SerializeField]
    private Status              status;                 // �÷��̾��� ���� (�̵��ӵ�, ü��)
    
    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI     txtWeaponName;          // ���� �̸�
    [SerializeField]
    private Image               imgWeaponIcon;          // ���� ������
    [SerializeField]
    private Sprite[]            spriteWeaponIcons;      // ���� �����ܿ� ���� Sprite �迭
    [SerializeField]
    private Vector2[]           sizeWeaponIcons;        // ���� �������� UI ũ�� �迭

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI     txtAmmo;                // (���� / �ִ�) ź�� �� ���

    [Header("Magazine")]
    [SerializeField]
    private GameObject          magazineUIPrefab;       // źâ UI ������
    [SerializeField]
    private Transform           magazineParent;         // źâ UI�� ��ġ�Ǵ� Panel
    [SerializeField]
    private int                 maxMagazineCount;       // ó�� �����ϴ� �ִ� źâ ��

    private List<GameObject>    magazineList;           // źâ UI ����Ʈ

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI     textHP;                 // �÷��̾��� ü���� ����ϴ� Text
    [SerializeField]
    private Image               imageBloodScreen;       // �÷��̾ ���ݹ޾��� �� ȭ�鿡 ǥ�õǴ� Image
    [SerializeField]
    private AnimationCurve      curveBloodScreen;

    private void Awake()
    {
        // �޼ҵ尡 ��ϵǾ� �ִ� �̺�Ʈ Ŭ����(weapon.xx)��
        // Invoke() �޼ҵ尡 ȣ��� �� ��ϵ� �޼ҵ�(�Ű�����)�� �����
        status.onHPEvent.AddListener(UpdateHPHUD);
    }
    public void SetupAllWeapon(WeaponBase[] weapons)
    {
        SetupMagazine();
        // ��� ������ ��� ������ �̺�Ʈ ���
        for(int i =0; i < weapons.Length; ++i)
        {
            weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
            weapons[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
        }
    }
    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;

        SetupWeapon();
    }
    private void SetupWeapon()
    {
        txtWeaponName.text = weapon.WeaponName.ToString();
        imgWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        imgWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
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
        for (int i = 0; i < maxMagazineCount; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
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
    private void UpdateHPHUD(int previous, int current)
    {
        textHP.text = "HP " + current;
        if(previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }
    private IEnumerator OnBloodScreen()
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }
}
