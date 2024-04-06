using UnityEngine;
using Utils.FactoryTool;
using System;
public class FactoryService : MonoBehaviour
{
    [SerializeField] private GroundTile _groundTilePrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Coin _coinPrefab;

    public Factory<GroundTile> groundTile { get; private set; }
    public Factory<Enemy> enemy { get; private set; }
    public Factory<Bullet> bullet { get; private set; }
    public Factory<Coin> coin { get; private set; }


    private void Awake()
    {
        groundTile = new Factory<GroundTile>(_groundTilePrefab, 10);
        enemy = new Factory<Enemy>(_enemyPrefab, 50);
        bullet = new Factory<Bullet>(_bulletPrefab, 100);
        coin = new Factory<Coin>(_coinPrefab, 10);
    }

    private void OnDestroy()
    {
        groundTile.Dispose();
        enemy.Dispose();
        bullet.Dispose();
        coin.Dispose();
    }
}
