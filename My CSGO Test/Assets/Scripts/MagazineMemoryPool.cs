using UnityEngine;

public class MagazineMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject magazinePrefab;
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(magazinePrefab);
    }
    public void SpawnMagazine(Vector3 position, Vector3 direction)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = Random.rotation;
        item.GetComponent<Magazine>().Setup(memoryPool, direction);
    }
}
