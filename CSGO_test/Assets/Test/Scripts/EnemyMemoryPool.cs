using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnPointPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float enemySpawnTime = 1;
    [SerializeField]
    private float enemySpawnLatency = 1;

    private MemoryPool spawnPointMemoryPool;
    private MemoryPool enemyMemoryPool;

    private int enemySpawnAtOnceOfNum = 1;
    private Vector2Int mapSize = new Vector2Int(100, 100);

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNum = 0;
        int maxNum     = 50;

        while(true)
        {
            // ���ÿ� enemySpawnAtOnceOfNum ���ڸ�ŭ ���� �����ǵ��� �ݺ��� ���
            for(int i = 0; i < enemySpawnAtOnceOfNum; ++ i)
            {
                GameObject item = spawnPointMemoryPool.ActivatePoolItem();
                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f), 0.5f,
                                                      Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f));
                StartCoroutine("SpawnEnemy", item);
            }
            currentNum++;
            if(currentNum >= maxNum)
            {
                currentNum = 0;
                enemySpawnAtOnceOfNum++;
            }
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    private IEnumerator SpawnEnemy(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� Point�� ��ġ�� ����
        GameObject item = enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }
}
