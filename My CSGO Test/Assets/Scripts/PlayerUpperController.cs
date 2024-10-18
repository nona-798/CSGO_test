using UnityEngine;

public class PlayerUpperController : MonoBehaviour
{
    private Animator animator;

    [Header("GameObject")]
    [SerializeField]
    private GameObject cam;

    [Header("Upper & Bottom Transform")]
    private Transform chestTransform;
    private Transform headTransform;
    private Vector3 chestOffset;
    private Vector3 headOffset;
    private Vector3 chestDir;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        chestTransform = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
        chestOffset = new Vector3(chestTransform.rotation.x, chestTransform.rotation.y, chestTransform.rotation.z);
        headOffset = new Vector3(headTransform.rotation.x, headTransform.rotation.y, headTransform.rotation.z);
    }
    private void LateUpdate()
    {
        BodyRotationUpdate(cam.transform);
    }
    private void BodyRotationUpdate(Transform rot)
    {
        chestDir = rot.position + rot.forward * 50; //+ new Vector3(0.5f, 0, 0.3f)

        chestTransform.LookAt(chestDir);
        headTransform.LookAt(chestDir);

        chestTransform.rotation = chestTransform.rotation * Quaternion.Euler(chestOffset);
        headTransform.rotation = headTransform.rotation * Quaternion.Euler(headOffset);
    }
}
