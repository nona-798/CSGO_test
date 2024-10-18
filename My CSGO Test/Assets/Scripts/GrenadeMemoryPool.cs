using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GrenadeType
{
    /// < summary > ����   0 : ����ź, 1 : ����ź, 2 : ����ź, 3 : ���̼���ź </summary>
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

    // ���⼭ �� �����Ϸ��� ���� ������ ��
}
