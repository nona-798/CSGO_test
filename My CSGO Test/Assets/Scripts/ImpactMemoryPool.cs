using UnityEngine;
public enum ImpactType {    Normal = 0, Target, InteractionObject  }
public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] impactPrefab;
    private MemoryPool[] memoryPool;

    private void Awake()
    {
        // 피격 이벤트가 여러 종류라면 종류별로 memory pool 생성
        memoryPool = new MemoryPool[impactPrefab.Length];
        for(int i = 0; i <impactPrefab.Length; ++i)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        // 부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if (hit.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if(hit.transform.CompareTag("ImpactTarget"))
        {
            OnSpawnImpact(ImpactType.Target, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("InteractionObject"))
        {
            Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.InteractionObject, hit.point, Quaternion.LookRotation(hit.normal), color);
        }
    }
    public void SpawnImpact(Collider ohter, Transform knifeTransform)
    {
        // 부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if (ohter.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (ohter.transform.CompareTag("ImpactTarget"))
        {
            OnSpawnImpact(ImpactType.Target, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (ohter.transform.CompareTag("InteractionObject"))
        {
            Color color = ohter.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.InteractionObject, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation), color);
        }
    }

    private void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject item = memoryPool[(int)type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(memoryPool[(int)type]);

        if(type == ImpactType.InteractionObject)
        {
            ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }
}
