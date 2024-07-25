using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionRadius = 10.0f;
    [SerializeField]
    private float explosionForce = 500.0f;
    [SerializeField]
    private float throwForce = 1000.0f;

    private int explosionDamage;
    private new Rigidbody rigidbody;

    public void Setup(int dmg, Vector3 rot)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rot * throwForce);

        explosionDamage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 폭발 범위에 있는 모든 오브젝트의 Collider 정보를 받아와 폭발 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in colliders)
        {
            // 폭발 범위에 부딪힌 오브젝트가 플레이어일 때 처리
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage((int)(explosionDamage));
                continue;
            }

            // 폭발 범위에 부딪힌 오브젝트가 적 캐릭터일 때 처리
            EnemyFSM enemy = hit.GetComponentInParent<EnemyFSM>();
            if(enemy != null)
            {
                enemy.TakeDamage((int)(explosionDamage));
                continue;
            }

            // 폭발 범위에 부딪힌 오브젝트가 상호작용 오브젝트이면 TakeDamage()로 피해를 줌
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)
            {
                interaction.TakeDamage((int)(explosionDamage));
            }

            // 중력을 가지고 있는 오브젝트이면 힘을 받아 밀려나도록
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // 수류탄 오브젝트 삭제
        Destroy(gameObject);
    }
}
