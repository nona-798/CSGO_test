using UnityEngine;

public class RotateMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamSpeed = 50.0f;

    private float limitMinX = -90;
    private float limitMaxX = 90;
    private float eulerAngleX;
    private float eulerAngleY;

    /// <summary> 플레이어 카메라 로테이션 & 플레이어 Y축 (좌우) 로테이션 함수 </summary>
    public void UpdateRotate(GameObject cam, float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamSpeed; // 마우스 좌우회전
        eulerAngleX -= mouseY * rotCamSpeed; // 마우스 상하 회전
        // 카메라 X축 회전 범위 제한
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        cam.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
    }
    /// <summary> 플레이어 X축 (상하) 로테이션 함수 </summary>
    public void UpdateBodyXRotate(float spineRot, float mouseY)
    {
        eulerAngleX -= mouseY * rotCamSpeed; // 마우스 상하 회전

        // 카메라 X축 회전 범위 제한
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        spineRot = eulerAngleX;
    }
    public Vector3 GetCamRotation(GameObject target)
    {
        return target.transform.rotation.eulerAngles;
    }
  
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
