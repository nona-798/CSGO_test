// ! ������ ������ ���� ������ �� �������� ����ϴ� �������� ����ü�� ��� �����ϸ�
//    ������ �߰�/������ �� ����ü�� �����ϱ� ������ �߰�/������ ���� ������ ������.

// ! [System.Serializable]�� �̿��� ����ȭ���� ������ �ٸ� Ŭ������ ������ �����Ǿ��� ��
//    Inspector View�� ��� ������ ����� ���� �ʴ´�.
// ! ����ü(struct)�� ���ÿ���, Ŭ����(class)�� �� ������ �޸� �Ҵ�� 
[System.Serializable]
public struct WeaponSetting
{
    public float attackRate;        // ���� �ӵ�
    public float attackDis;         // ���� ��Ÿ�
    public bool  isAutomaticAttack; // ���� ���� ����
}

