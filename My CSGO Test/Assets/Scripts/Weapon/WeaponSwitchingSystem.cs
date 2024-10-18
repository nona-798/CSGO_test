using UnityEngine;

public class WeaponSwitchingSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private PlayerHUD playerHUD;

    [SerializeField]
    private WeaponBase[] weapons;

    private WeaponBase currentWeapon;
    private WeaponBase previousWeapon;

    private void Awake()
    {
        // ���� ���� ����� ���� ���� ���� ���� ��� ���⸦ ���
        playerHUD.SetupAllWeapons(weapons);

        // ���� ���� ���� ��� ���⸦ ������ �ʰ� ����
        for(int i = 0; i < weapons.Length; ++i)
        {
            if(weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        // Sub ���⸦ ���� ��� ����� ����
        SwitchingWeapon(WeaponType.Sub);
    }
    // Update is called once per frame
    private void Update()
    {
        UpdateSwitch();
    }

    /// < summary > 
    /// built in UpdateSwitch() method
    /// int data; bool result = int.TryParse(string Key, out data)
    /// key ���ڿ��� ���ڷ� ����ȯ�ؼ� data�� ����
    /// �����ϸ� result : true, ���н� result : false
    /// </summary>
    private void UpdateSwitch()
    {
        if (!Input.anyKeyDown) return;
        // 1~4 ����Ű�� ������ ���� ��ü
        int inputIndex = 0;
        if( int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 5))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }
    }
    private void SwitchingWeapon(WeaponType weaponType)
    {
        if(weapons[(int)weaponType] == null)
        {
            return;
        }

        if (currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }

        currentWeapon = weapons[(int)weaponType];

        if (currentWeapon == previousWeapon) return;

        playerController.SwitchWeapon(currentWeapon);
        playerHUD.SwitchingWeapon(currentWeapon);

        if (previousWeapon != null)
        {
            previousWeapon.gameObject.SetActive(false);
        }

        currentWeapon.gameObject.SetActive(true);
    }
}
