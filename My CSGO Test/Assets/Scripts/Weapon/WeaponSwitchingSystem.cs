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
        // 무기 정보 출력을 위해 현재 소지 중인 모든 무기를 등록
        playerHUD.SetupAllWeapons(weapons);

        // 현재 소지 중인 모든 무기를 보이지 않게 설정
        for(int i = 0; i < weapons.Length; ++i)
        {
            if(weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        // Sub 무기를 현재 사용 무기로 설정
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
    /// key 문자열을 숫자로 형변환해서 data에 저장
    /// 성공하면 result : true, 실패시 result : false
    /// </summary>
    private void UpdateSwitch()
    {
        if (!Input.anyKeyDown) return;
        // 1~4 숫자키를 누르면 무기 교체
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
