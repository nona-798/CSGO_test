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
        // �þ� �¿� �̵�
        eulerAngleY += mouseX * RotateSpeed;
        // �þ� ���� �̵�
        eulerAngleX -= mouseY * RotateSpeed;

        // ī�޶� X�� ȸ�� ����
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
