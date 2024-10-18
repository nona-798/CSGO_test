using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem particle;
    private MemoryPool memoryPool;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }
    private void Update()
    {
        // 만일 파티클이 재생중이 아니라면 삭제
        if(particle.isPlaying == false)
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
