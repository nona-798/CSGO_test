public enum WeaponName {  AK47 = 0, Glock17, CombatKnife, HandGranade  }
[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;
    public int currentAmmo;
    public int reloadAmmo;
    public int maxAmmo;
    public int damage;
    public float atkRate;
    public float atkRange;
    public bool IsAutomatic;
}
