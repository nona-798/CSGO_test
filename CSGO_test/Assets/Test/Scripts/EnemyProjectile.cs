using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private MovementTransform movement;
    private float projectileDistance = 30;
    // Start is called before the first frame update
    public void Setup(Vector3 pos)
    {
        movement = GetComponent<MovementTransform>();

        StartCoroutine("OnMove", pos);
    }
    private IEnumerator OnMove(Vector3 targetPos)
    {
        Vector3 start = transform.position;

        movement.MoveTo((targetPos - transform.position).normalized);
        while(true)
        {
            if(Vector3.Distance(transform.position,start) >= projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");

            Destroy(gameObject);
        }
    }
}
