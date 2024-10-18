using UnityEngine;
[System.Serializable]
public class IStatus : MonoBehaviour
{
    [Header("Walk,Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    // 속도 값 Set, Get 변수
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
}
