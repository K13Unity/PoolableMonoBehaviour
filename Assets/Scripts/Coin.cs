using System;
using UnityEngine;
using Utils.FactoryTool;

public class Coin : PoolableMonoBehaviour
{
    [SerializeField] float _turnSpeed = 90f;
    [SerializeField] float _moveSpeed = 5f;

    private Transform _playerTransform;

    public event Action <Coin> OnCoinArrived;

    public void MoveToPlayer(PlayerController player)
    {
        _playerTransform =  player.transform;
    }

    void Update()
    {
        if (_playerTransform != null) 
        {
            var distance = Vector3.Distance(transform.position, _playerTransform.position);
            if (distance > 0.1f)
            {
                Vector3 direction = (_playerTransform.position - transform.position).normalized;
                transform.position += direction * _moveSpeed * Time.deltaTime;
                transform.Rotate(0, 0, _turnSpeed * Time.deltaTime);
            }
            else
            {
                OnCoinArrived?.Invoke(this); 
            }
        }
    }
   
}
