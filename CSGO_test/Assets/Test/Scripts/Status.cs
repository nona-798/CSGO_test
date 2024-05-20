using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    [Header("Walk,Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("HP")]
    [SerializeField]
    private int maxHP = 100;
    private int currentHP;

    // 속도 값 SetGet 변수
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Awake()
    {
        currentHP = maxHP;
    }
    public bool DecreaseHP(int dmg)
    {
        int previousHP = currentHP;

        currentHP = currentHP - dmg > 0 ? currentHP - dmg : 0;

        onHPEvent.Invoke(previousHP, currentHP);

        if(currentHP <= 0)
        {
            return true;
        }

        return false;
    }
}
