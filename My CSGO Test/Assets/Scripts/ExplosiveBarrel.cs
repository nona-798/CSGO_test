using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : InteractionObject
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionDelayTime = 0.3f;
    [SerializeField]
    private float explosionRadious = 10.0f;
    [SerializeField]
    private float explosionForce = 100.0f;

    private bool isExplode = false;

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("ExplodeBarrel");
        }
    }
    private IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        isExplode = true;

        // 폭발 이펙트 생성
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, 
            new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), 
            transform.rotation);

        // 폭발 범위에 있는 모든 오브젝트의 Collider 정보를 받아와 폭발 피해 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious);
        foreach(Collider hit in colliders)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(75);
                continue;
            }
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)
            {
                interaction.TakeDamage(75);
                continue;
            }
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadious);
            }
        }
        Destroy(gameObject);
    }
}
