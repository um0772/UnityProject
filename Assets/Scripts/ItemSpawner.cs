using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // 생성할 아이템 프리팹
    public float spawnInterval = 5f; // 아이템 생성 간격
    public Vector2 spawnAreaMin; // 스폰 영역 최소 좌표
    public Vector2 spawnAreaMax; // 스폰 영역 최대 좌표
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
