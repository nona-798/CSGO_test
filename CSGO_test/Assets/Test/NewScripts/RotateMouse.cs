using UnityEngine;

public class RotateMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamSpeed = 50.0f;

    private float limitMinX = -90;
    private float limitMaxX = 90;
    private float eulerAngleX;
    private float eulerAngleY;

    /// <summary> �÷��̾� ī�޶� �����̼� & �÷��̾� Y�� (�¿�) �����̼� �Լ� </summary>
    public void UpdateRotate(GameObject cam, float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamSpeed; // ���콺 �¿�ȸ��
        eulerAngleX -= mouseY * rotCamSpeed; // ���콺 ���� ȸ��
        // ī�޶� X�� ȸ�� ���� ����
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        cam.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
    }
    /// <summary> �÷��̾� X�� (����) �����̼� �Լ� </summary>
    public void UpdateBodyXRotate(float spineRot, float mouseY)
    {
        eulerAngleX -= mouseY * rotCamSpeed; // ���콺 ���� ȸ��

        // ī�޶� X�� ȸ�� ���� ����
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
