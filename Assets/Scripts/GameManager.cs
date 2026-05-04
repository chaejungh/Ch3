using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    
    [SerializeField]
    float itemSpawnInterval = 2f;
    
    [SerializeField]
    Vector2 itemSpawnYRange = new Vector2(-2f, 2f);
    
    [SerializeField]
    Vector2 itemSpawnXRange = new Vector2(-2f, 2f);

    [SerializeField]
    Transform itemParent;
    
    float itemSpawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPrefab == null)
            return;
        
        itemSpawnTimer += Time.deltaTime;

        if (itemSpawnTimer < itemSpawnInterval)
            return;
        
        itemSpawnTimer = 0f;
        SpawnItem();
    }


    void SpawnItem()
    {
        float spawnX = Random.Range(itemSpawnXRange.x, itemSpawnXRange.y);
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, itemParent);
    }
}
