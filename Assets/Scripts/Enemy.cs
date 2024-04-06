using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.FactoryTool;

public class Enemy : PoolableMonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private Text _healthText;
    [SerializeField] private float _moveSpeed = 2f;

    [SerializeField] private float _rotationSpeed = 1f;
    Transform _playerTransform;

    public event Action OnEnemyDeath;

    void Start()
    {
        int[] possibleHitPoints = { 10, 20, 30 };
        _health = possibleHitPoints[UnityEngine.Random.Range(0, possibleHitPoints.Length)];
        UpdateHealthText();
    }
    public void Initialize(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            Vector3 direction = (_playerTransform.position - transform.position).normalized;

            transform.position += direction * _moveSpeed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
        }

        if (transform.position.y < -5)
        {
            KillEnemy();
        }
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        UpdateHealthText();
        if (_health <= 0)
        {
            KillEnemy();
        }
    }
    void UpdateHealthText()
    {
        _healthText.text = _health.ToString();
    }
    public void KillEnemy()
    {
        OnEnemyDeath?.Invoke();
        
        Destroy(gameObject);
    }
}
