using UnityEngine;

public class BulletholeMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletholePrefab;
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(bulletholePrefab);
    }

    public void SpawnBullethole(RaycastHit hit)
    {
        OnSpawnBullethole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
    }
    private void OnSpawnBullethole(Vector3 position, Quaternion rotation)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Bullethole>().Setup(memoryPool);
    }
}
