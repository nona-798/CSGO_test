using UnityEngine;

public class DestructibleObject : InteractionObject
{
    [Header("Destructible Barrel")]
    [SerializeField]
    private GameObject destructibleBarrelPieces;

    private bool isDestroyed = false;
    public override void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0 && isDestroyed == false)
        {
            isDestroyed = true;

            Instantiate(destructibleBarrelPieces, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
