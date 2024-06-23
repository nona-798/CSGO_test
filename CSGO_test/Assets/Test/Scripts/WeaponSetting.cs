// ! ������ ������ ���� ������ �� �������� ����ϴ� �������� ����ü�� ��� �����ϸ�
//    ������ �߰�/������ �� ����ü�� �����ϱ� ������ �߰�/������ ���� ������ ������.

// ! [System.Serializable]�� �̿��� ����ȭ���� ������ �ٸ� Ŭ������ ������ �����Ǿ��� ��
//    Inspector View�� ��� ������ ����� ���� �ʴ´�.
// ! ����ü(struct)�� ���ÿ���, Ŭ����(class)�� �� ������ �޸� �Ҵ�� 
public enum WeaponName {  AssaultRifle = 0, Glock17, CombatKnife, HandGrenade  }

[System.Serializable]
public struct WeaponSetting
{                                          
    public WeaponName   weaponName;        // ���� �̸�
    public int          damage;            // ���� ���ݷ�
    public int          currentMag;        // ���� źâ ��
    public int          maxMag;            // �ִ� źâ ��
    public int          currentAmmo;       // ���� ź�� ��
    public int          maxAmmo;           // �ִ� ź�� ��
    public float        attackRate;        // ���� �ӵ�
    public float        attackDis;         // ���� ��Ÿ�
    public bool         isAutomaticAttack; // ���� ���� ����
}

