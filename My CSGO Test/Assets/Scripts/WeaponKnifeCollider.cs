using System.Collections;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{
    [SerializeField]
    private ImpactMemoryPool impactMemoryPool;
    [SerializeField]
    private Transform knifeTransform;

    private new Collider collider;
    private int damage;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        this.damage = damage;
        collider.enabled = true;

        StartCoroutine("DisableByTime", 0.1f);
    }

    private IEnumerator DisableByTime(float time)
    {
        yield return new WaitForSeconds(time);
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        impactMemoryPool.SpawnImpact(other, knifeTransform);

        if (other.CompareTag("ImpactTarget"))
        {
            //other.GetComponentInParent
        }
        else if (other.CompareTag("InteractionObject"))
        {
            other.GetComponent<InteractionObject>().TakeDamage(damage);
        }
    }
}
