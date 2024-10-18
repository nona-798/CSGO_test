using UnityEngine;

public class DestructibleBarrel : InteractionObject
{
    [Header("Destructible Barrel")]
    [SerializeField]
    private GameObject destructibleBarrelPices;

    private bool isDestroy = false;
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0 && isDestroy == false)
        {
            isDestroy = true;

            Instantiate(destructibleBarrelPices, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
