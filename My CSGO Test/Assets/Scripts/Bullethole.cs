using System.Collections;
using UnityEngine;

public class Bullethole : MonoBehaviour
{
    [SerializeField]
    private float deactivateTime = 45;

    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;

        StartCoroutine("BulletholeDeactivateAfterTime");
    }
    private IEnumerator BulletholeDeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
