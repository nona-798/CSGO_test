using UnityEngine;
[System.Serializable]
public class IStatus : MonoBehaviour
{
    [Header("Walk,Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    // �ӵ� �� Set, Get ����
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
}
