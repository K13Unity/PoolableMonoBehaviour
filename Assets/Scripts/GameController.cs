using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

[System.Serializable]
public class EnemySpawnConfig
{
    public List<Vector3> spawnPositions = new List<Vector3>();
}

public class GameController : MonoBehaviour
{ 
    public static GameController instance;
    [SerializeField] private FactoryService _factoryService;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private List<EnemySpawnConfig> enemySpawnConfigs = new List<EnemySpawnConfig>();

    [SerializeField] private int desiredTileCount = 1;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float timeSinceLastShot = 0f;
    [SerializeField] private Text _scoreText;


    private int _playerCoins;

    private List<GroundTile> groundTiles = new List<GroundTile>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnNewTile();
    }

    void Update()
    {
        Shoot();
    }

    public void AddScore(int value)
    {
        _playerCoins += value;
        UpdateScoreText();
    }

    public void BuyItem(int itemPrice)
    {
        if (_playerCoins >= itemPrice)
        {
            _playerCoins -= itemPrice;
            UpdateScoreText();
        }
        else
        {
            Debug.Log("Not enough");
        }
    }

    private void UpdateScoreText()
    {
        _scoreText.text = "" + _playerCoins;
    }

    public void SpawnNewTile()
    {
        for (int i = 0; i < desiredTileCount; i++)
        {
            CreateTile();
        }
    }

    private void CreateTile()
    {
        var tile = _factoryService.groundTile.Produce();
        tile.transform.position = GetPisition();

        groundTiles.Add(tile);
        CreateEnemyGroup(tile, 0);
        tile.OnPlayerExit += OnExit;
    }

    private Vector3 GetPisition()
    {
        if (groundTiles.Count == 0)
        {
            return Vector3.zero;
        }
        else
        {
            var tile = groundTiles[groundTiles.Count - 1];
            return new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + tile.transform.localScale.z * 10);
        }
    }

    private async void OnExit()
    {
        Debug.Log("Exit");
        await Task.Delay(3000);
        var tile = groundTiles[0];
        tile.OnPlayerExit -= OnExit;
        groundTiles.RemoveAt(0);
        _factoryService.groundTile.Release(tile);
        CreateTile();
    }

    private void CreateEnemyGroup(GroundTile tile, int configIndex)
    {
        if(tile.transform.position == Vector3.zero)
        {
            return;
        }

        EnemySpawnConfig config = enemySpawnConfigs[configIndex];

        foreach (Vector3 position in config.spawnPositions)
        {
            var enemy = _factoryService.enemy.Produce();
            enemy.transform.position = position + tile.transform.position;
            enemy.OnEnemyDeath += () => SpawnCoin(enemy.transform.position);
            enemy.Initialize(_playerController.transform);
        }
    }

    public void DeleteBullet(Bullet bullet)
    {
        _factoryService.bullet.Release(bullet);
    }

    void Shoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= 1f / fireRate)
        {
            var bullet = _factoryService.bullet.Produce();
            bullet.Fire(firePoint.transform.position, firePoint.transform.rotation);
            timeSinceLastShot = 0f;
        }
    }

    public void SpawnCoin(Vector3 spawnPoint)
    {
        var coin = _factoryService.coin.Produce();
        coin.MoveToPlayer(_playerController);
        coin.transform.position = spawnPoint;
        coin.OnCoinArrived += OnCoinPickup;
    }

    void OnCoinPickup(Coin coin)
    {
        AddScore(10);
        coin.OnCoinArrived -= OnCoinPickup;
        _factoryService.coin.Release(coin);
    }
}

