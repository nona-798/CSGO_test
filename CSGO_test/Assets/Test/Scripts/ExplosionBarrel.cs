using System.Collections;
using UnityEngine;

public class ExplosionBarrel : InteractionObject
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionDelayTime = 0.3f;
    [SerializeField]
    private float explosionRadious = 10.0f;
    [SerializeField]
    private float explosionForce = 1000.0f;

    private bool isExplode = false;

    public override void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("ExplodeBarrel");
        }
    }

    private IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        // ��ó�� ������ ������ �ٽ� ���� ������ �Ͷ߸����� �� ��(StackOverFlow ����)
        isExplode = true;

        // ���� ����Ʈ ����
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        // ���� ������ �ִ� ��� ������Ʈ�� Collider ������ �޾ƿ� ���� ȿ�� ó��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious);
        foreach(Collider hit in colliders)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(50);
                continue;
            }

            // ���� ������ �ε��� ������Ʈ�� �� ĳ������ �� ó��
            EnemyFSM enemy = hit.GetComponentInParent<EnemyFSM>();
            if( enemy != null)
            {
                enemy.TakeDamage(300);
                continue;
            }

            // ���� ������ �ε��� ������Ʈ�� ��ȣ�ۿ� ������Ʈ�̸� TakeDamage()�� ���ظ� ��
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)
            {
                interaction.TakeDamage(300);
            }

            // �߷��� ������ �ִ� ������Ʈ�̸� ���� �޾� �з�������
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if( rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadious);
            }
        }

        // ���� ������Ʈ ����
        Destroy(gameObject);
    }
}
