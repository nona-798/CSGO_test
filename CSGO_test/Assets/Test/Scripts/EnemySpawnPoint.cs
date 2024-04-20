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
            // time 값에 따라 0부터 length 사이의 값이 반환된다.
            // time 값이 계속 증가할 때 length까지는 t값을 반환하고,
            // time이 length보다 커졌을 때 순차적으로 0까지 -, length까지 +를 반복
            meshRenderer.material.color = color;
            
            yield return null;
        }
    }
}
