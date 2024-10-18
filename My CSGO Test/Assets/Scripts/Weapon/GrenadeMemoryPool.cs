using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GrenadeType
{
    /// < summary > 종류   0 : 수류탄, 1 : 섬광탄, 2 : 연막탄, 3 : 소이수류탄 </summary>
    Grenade = 0, Flash, Smoke, Incendiary
}
public class GrenadeMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] grenadePrefab;
    private MemoryPool[] memoryPool;

    private int maxGrenadeType = 1;
    private int maxGrenade = 3;
    private void Awake()
    {
        grenadePrefab = new GameObject[3];
        memoryPool = new MemoryPool[grenadePrefab.Length];
        for(int i = 0; i < maxGrenade; ++i )
        {
            memoryPool[i] = new MemoryPool(grenadePrefab[i]);
        }
    }

    // 여기서 더 개발하려면 상점 만들어야 함
}
