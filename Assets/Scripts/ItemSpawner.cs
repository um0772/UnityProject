using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ������ ������
    public float spawnInterval = 5f; // ������ ���� ����
    public Vector2 spawnAreaMin; // ���� ���� �ּ� ��ǥ
    public Vector2 spawnAreaMax; // ���� ���� �ִ� ��ǥ
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnItem", spawnInterval, spawnInterval);

    }
    void SpawnItem()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
