using UnityEngine;
using Utils.FactoryTool;

public class Bullet : PoolableMonoBehaviour
{
    [SerializeField] float _destroyDistance = 10f;
    [SerializeField] int damage = 1;
    [SerializeField] float _speed = 5f;

    private bool _isFired = false;
    private Vector3 _initialPosition;

    private Vector3 _firstPos;
    private Vector3 _secondPos;


    void Update()
    {
        if (_isFired)
        {
            transform.position += transform.forward * _speed * Time.deltaTime;

            _secondPos = transform.position;
            if (Physics.Raycast(_secondPos, Vector3.Normalize(_secondPos - _firstPos), out var hit,
                Vector3.Distance(_secondPos, _firstPos)))
            {
                _firstPos = _secondPos;
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(1);
                    Destroy(gameObject);
                }
            }
        }
        CheckDistance();
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, _initialPosition);

        if (distance > _destroyDistance)
        {
            DestroyBullet();
        }
    }

    public void Fire(Vector3 startPos, Quaternion startRotation)
    {
        _firstPos = startPos;
        _initialPosition = startPos;
        _isFired = true;

        transform.position = startPos;  
        transform.rotation = startRotation;
    }
    public override void Dispose()
    {
        _isFired = false;
        _firstPos = Vector3.zero; 
        _initialPosition = Vector3.zero;
        _secondPos = Vector3.zero;
    }

    void DestroyBullet()
    {
        GameController.instance.DeleteBullet(this);
    }
}
