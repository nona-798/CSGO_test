using System.Collections;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float deactiveTime = 5;                   // ź�� ���� �� ��Ȱ��ȭ �Ǵ� �ð�
    [SerializeField]
    private float casingSpin = 1;                     // ź�ǰ� ȸ���ϴ� �ӷ� ���
    [SerializeField]
    private AudioClip[] audioClips;                   // ź�ǰ� �ٴڿ� �ε����� �� ����Ǵ� ����

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;

        // ź���� �̵� �ӷ°� ȸ�� �ӷ� ����
        rigidbody3D.velocity = new Vector3(direction.x, 1, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin));

        // ź�� �ڵ� ��Ȱ��ȭ�� ���� �ڷ�ƾ ����
        StartCoroutine("CasingDeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���� ź�� ���� �� ������ ���带 ����
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator CasingDeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactiveTime);

        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
