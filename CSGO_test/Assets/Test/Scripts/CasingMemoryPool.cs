using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject casingPrefab; // 탄피 오브젝트
    private MemoryPool memoryPool;   // 탄피 메모리풀

    private void Awake()
    {
        memoryPool = new MemoryPool(casingPrefab);
    }

    public void SpawnCasing(Vector3 pos, Vector3 dir)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = pos;
        item.transform.rotation = Random.rotation;
        item.GetComponent<Casing>().Setup(memoryPool, dir);
    }
}
