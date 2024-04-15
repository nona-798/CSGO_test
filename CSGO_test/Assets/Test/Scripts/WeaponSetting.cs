// ! 무기의 종류가 여러 종류일 때 공용으로 사용하는 변수들은 구조체로 묶어서 정의하면
//    변수가 추가/삭제될 때 구조체에 선언하기 때문에 추가/삭제에 대한 관리가 용이함.

// ! [System.Serializable]을 이용해 직렬화하지 않으면 다른 클래스의 변수로 생성되었을 때
//    Inspector View에 멤버 변수의 목록이 뜨지 않는다.
// ! 구조체(struct)는 스택영역, 클래스(class)는 힙 영역에 메모리 할당됨 
[System.Serializable]
public struct WeaponSetting
{
    public float attackRate;        // 공격 속도
    public float attackDis;         // 공격 사거리
    public bool  isAutomaticAttack; // 연속 공격 여부
}

