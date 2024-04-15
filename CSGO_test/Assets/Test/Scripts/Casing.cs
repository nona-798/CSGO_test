using System.Collections;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float deactivateTime = 5.0f;      // 탄피 등장 후 비활성까지의 시간
    [SerializeField]
    private float casingSpin = 1.0f;          // 탄피가 회전하는 속력 계수
    [SerializeField]
    private AudioClip[] audioClips;           // 탄피가 어떤 물체에 부딪힐 때 재생되는 사운드

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool, Vector3 dir)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;

        // 탄피의 이동 속력과 회전 속력 설정
        rigidbody3D.velocity = new Vector3(dir.x, 1.0f, dir.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),    // x
                                                  Random.Range(-casingSpin, casingSpin),    // y
                                                  Random.Range(-casingSpin, casingSpin));   // z
        // 탄피 자동 비활성화를 위한 코루틴 실행
        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 여러 개의 탄피 사운드 중 임의의 사운드 선택
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);

        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
