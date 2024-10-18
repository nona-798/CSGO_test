using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField]
    private float magazineSpin = 1;

    private Rigidbody rigidbody3D;
    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        memoryPool = pool;

        rigidbody3D.velocity = new Vector3(direction.x, 1, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-magazineSpin, magazineSpin),
                                                  Random.Range(-magazineSpin, magazineSpin),
                                                  Random.Range(-magazineSpin, magazineSpin));
    }
}
