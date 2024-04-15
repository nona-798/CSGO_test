using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject casingPrefab; // ź�� ������Ʈ
    private MemoryPool memoryPool;   // ź�� �޸�Ǯ

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
