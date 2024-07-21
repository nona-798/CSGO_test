using System.Collections;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{
    [SerializeField]
    private ImpactMemoryPool impactMemoryPool;
    [SerializeField]
    private Transform knifeTransform;

    private new Collider collider;
    private int dmg;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void StartCollider(int dmg)
    {
        this.dmg = dmg;
        collider.enabled = true;

        StartCoroutine("DisablebyTime", 0.1f);
    }
    private IEnumerator DisablebyTime(float time)
    {
        yield return new WaitForSeconds(time);

        collider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        impactMemoryPool.SpawnImpact(other, knifeTransform);

        if(other.CompareTag("ImpactEnemy"))
        {
            other.GetComponentInParent<EnemyFSM>().TakeDamage(dmg);
        }
        else if(other.CompareTag("InteractionObject"))
        {
            other.GetComponent<InteractionObject>().TakeDamage(dmg);
        }
    }
}
