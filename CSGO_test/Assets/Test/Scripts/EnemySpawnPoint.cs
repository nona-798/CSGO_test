using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 4;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("OnFadeEffect");
    }

    private void OnDisable()
    {
        StopCoroutine("OnFadeEffect");
    }
    private IEnumerator OnFadeEffect()
    {
        while(true)
        {
            Color color = meshRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * fadeSpeed, 1));
            // float f = Mathf.Pingpong(float t, float Length);
            // time ���� ���� 0���� length ������ ���� ��ȯ�ȴ�.
            // time ���� ��� ������ �� length������ t���� ��ȯ�ϰ�,
            // time�� length���� Ŀ���� �� ���������� 0���� -, length���� +�� �ݺ�
            meshRenderer.material.color = color;
            
            yield return null;
        }
    }
}
