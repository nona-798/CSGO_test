using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float RotateSpeed = 50.0f;

    private float limitMinX = -85.0f;
    private float limitMaxX = 85.0f;
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(GameObject obj, float mouseX, float mouseY)
    {
        // 시야 좌우 이동
        eulerAngleY += mouseX * RotateSpeed;
        // 시야 상하 이동
        eulerAngleX -= mouseY * RotateSpeed;

        // 카메라 X축 회전 제한
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        obj.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= -360;

        return Mathf.Clamp(angle, min, max);
    }
}
