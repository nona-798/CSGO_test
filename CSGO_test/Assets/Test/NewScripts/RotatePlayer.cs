using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    private Animator anim;
    private Transform spine;
    [SerializeField]
    private float rotCamSpeed = 50.0f;

    private float limitMinX = -90;
    private float limitMaxX = 90;
    private float eulerAngleX;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        spine = anim.GetBoneTransform(HumanBodyBones.Spine);
    }
    public void UpdateRotate(float mouseY)
    {
        eulerAngleX -= mouseY * rotCamSpeed; // 마우스 상하 회전

        // 카메라 X축 회전 범위 제한
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
