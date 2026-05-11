using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    int score = 0;
    public int Score => score;

    public void AddScore(int amount = 1)
    {
        score += amount;
        Debug.Log($"Score:  {score}");
        scoreText.text = $"Score: {score}";
    }

    void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    GameObject itemPrefab;
    
    [SerializeField]
    GameObject ObstclePrefab;
    
    [SerializeField]
    TextMeshProUGUI scoreText;
    
    [SerializeField]
    TextMeshProUGUI distanceText;
    
    [SerializeField]
    float itemSpawnInterval = 2f;
    
    [SerializeField]
    Vector2 itemSpawnYRange = new Vector2(-2f, 2f);
    
    [SerializeField]
    Vector2 itemSpawnXRange = new Vector2(-2f, 2f);

    [SerializeField]
    Transform itemParent;
    
    [SerializeField]
    public float scrollSpeed = 10f;
    
    [Header("Speed Scale")]
    [SerializeField]
    float speedIncreaseRate = 0.5f;
    
    [SerializeField]
    float maxScrollSpeed = 30f;
    
    [SerializeField]
    public Camera targetCamera;
    
    float spawnTimer;
    int itemSpawnCount;
    
    [SerializeField]
    const int MAXHP = 3;
    int hp;
    bool isGameOver = false;
    
    [SerializeField]
    int itemsPerObstacle=10;
    
    [SerializeField]
    GameObject[] hpSprite;
    
    float distance =0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = MAXHP;
        // hpSprite=new GameObject[MAXHP];
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver) return;
        
        scrollSpeed = Mathf.Min(scrollSpeed + speedIncreaseRate * Time.deltaTime, maxScrollSpeed);
        Debug.Log($"Scroll Speed: {scrollSpeed}");
        
        distance += scrollSpeed * Time.deltaTime;
        if (distanceText != null)
        {
            distanceText.text = $"Distance:{(int)distance}M";
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer < itemSpawnInterval)
            return;
        
        spawnTimer = 0f;
        itemSpawnCount++;
        if (itemSpawnCount >= itemsPerObstacle)
        {
            itemSpawnCount = 0;
            SpawnObstacle();
        }
        else
        {
            SpawnItem();
        }
    }

 
    public void TakeDamage()
    {
        if (isGameOver) return;

        hp--;
        hpSprite[hp].SetActive(false);
        
        Debug.Log($"HP: {hp}");
        
        if (hp <= 0)
            GameOver();
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over");
    }

    void SpawnObstacle()
    {
        float spawnX = GetCameraRightX();
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject obstacle = Instantiate(ObstclePrefab, spawnPosition, Quaternion.identity, itemParent);
        
        ItemMover mover = obstacle.GetComponent<ItemMover>();
        if (mover == null)
        {
            mover = obstacle.AddComponent<ItemMover>();
        }

        mover.scrollSpeed = scrollSpeed;
        mover.targetCamera = targetCamera != null ? targetCamera : Camera.main;
        mover.isObstacle = true;
    }
    
    


    void SpawnItem()
    {
        float spawnX = GetCameraRightX();
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, itemParent);
        
        ItemMover mover = item.GetComponent<ItemMover>();
        if (mover == null)
        {
            mover = item.AddComponent<ItemMover>();
        }

        mover.scrollSpeed = scrollSpeed;
        mover.targetCamera = targetCamera != null ? targetCamera : Camera.main;
    }
    
    
    
    float GetCameraRightX()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return 10f;
        
        float distance = Mathf.Abs(cam.transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, distance)).x;
    }
    
}
