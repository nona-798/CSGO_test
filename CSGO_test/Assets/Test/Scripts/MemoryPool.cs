using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    // �޸� Ǯ�� �����Ǵ� ������Ʈ ����
    private class PoolItem
    {
        public bool isActive;                    // gameObject�� Ȱ��ȭ ��Ȱ��ȭ ����
        public GameObject gameObject;            // ȭ�鿡 ���̴� ���� gameObject
    }

    private int increaseCount = 5;               // ������Ʈ�� ������ �� Instantiate()�� �߰� �����Ǵ� ������Ʈ ����
    private int maxCount;                        // ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ����
    private int activeCount;                     // ���� ���ӿ� ���ǰ� �ִ� (Ȱ��ȭ) ������Ʈ ����

    private GameObject poolObject;               // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList;         // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

    public int MaxCount => maxCount;             // �ܺο��� ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ
    public int ActiveCount => activeCount;       // �ܺο��� ���� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ ���� Ȯ�� ���� ������Ƽ

    /// <summary> ������Ʈ�� �ӽ÷� �����Ǵ� ��ġ </summary>
    private Vector3 tempPos = new Vector3(48, 1, 48);

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstatiateObjects();
    }
    /// <summary> increaseCount ������ ������Ʈ�� ���� </summary>
    public void InstatiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.transform.position = tempPos;
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }
    ///<summary> ���� ��������(Ȱ��/��Ȱ��) ��� ������Ʈ�� ���� </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }
    /// <summary> poolItemList�� ����Ǿ� �ִ� ������Ʈ�� Ȱ��ȭ�ؼ� ���.
    /// ���� ��� ������Ʈ�� ������̸� InstantiateObjects()�� �߰� ���� </summary>
    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;
        // ���� �����ؼ� �����ϴ� ��� ������Ʈ ������ ���� Ȱ��ȭ ������ ������Ʈ ���� ��
        // ��� ������Ʈ�� Ȱ��ȭ �����̸� ���ο� ������Ʈ �ʿ�
        if(maxCount == activeCount)
        {
            InstatiateObjects();
        }

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if(poolItem.isActive == false)
            {
                activeCount ++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }
        return null;
    }

    /// <summary> ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ���� </summary>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.gameObject.transform.position = tempPos;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    /// <summary> ���ӿ� ������� ��� ������Ʈ�� ��Ȱ��ȭ ���·� ���� </summary> 
    public void DeactivateAllPoolItem()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.gameObject.transform.position = tempPos;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
