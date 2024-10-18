using UnityEngine;
[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class APEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class PlayerStatus : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();
    [HideInInspector]
    public APEvent onAPEvent = new APEvent();

    [Header("Walk, Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("Health")]
    [SerializeField]
    private int maxHP = 100;
    private int currentHP;

    [Header("Armor")]
    [SerializeField]
    private int maxAP = 100;
    private int currentAP;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
    public int CurrentAP => currentAP;
    public int MaxAP => maxAP;

    private void Awake()
    {
        currentHP = maxHP;
        currentAP = 0;
    }
    public bool DeaceaseHP(int damage)
    {
        int previousHP = currentHP;
        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);

        if(currentHP <= 0)
        {
            return true;
        }
        return false;
    }
}
