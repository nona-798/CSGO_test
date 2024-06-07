using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                               // 적의 목표 (플레이어)
    [SerializeField]                                        
    private GameObject enemySpawnPointPrefab;               // 적이 등장하기 전 적의 등장 위치
    [SerializeField]                                        
    private GameObject enemyPrefab;                         // 생성되는 적 프리팹
    [SerializeField]                                        
    private float enemySpawnTime = 1;                       // 적 생성 주기
    [SerializeField]                                        
    private float enemySpawnLatency = 1;                    // 타일 생성 후 적이 등장하기까지의 대기 시간

    private MemoryPool spawnPointMemoryPool;                // 적 등장 위치를 알려주는 오브젝트 생성, 활성/비활성 관리
    private MemoryPool enemyMemoryPool;                     // 적 생성, 활성/비활성 관리

    private int enemySpawnAtOnceOfNum = 1;                  // 동시에 생성되는 적의 숫자
    private Vector2Int mapSize = new Vector2Int(100, 100);  // 맵 크기

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
            // 동시에 enemySpawnAtOnceOfNum 숫자만큼 적이 생성되도록 반복문 사용
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

        // 적 오브젝트를 생성하고, 적의 위치를 Point의 위치로 설정
        GameObject item = enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;

        item.GetComponent<EnemyFSM>().Setup(target, this);

        // 타일 오브젝트를 비활성화
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }
    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}
