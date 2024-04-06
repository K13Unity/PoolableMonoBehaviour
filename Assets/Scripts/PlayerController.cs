using UnityEngine;
using Utils.FactoryTool;

public class PlayerController : PoolableMonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] float _horizontalSpeed = 1.0f;
   
    float _horizontalInput = 2;


    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        Vector3 forwardMove = transform.forward * _speed * Time.deltaTime;
        Vector3 horizontalMove = transform.right * _horizontalInput * _speed * Time.deltaTime;
        transform.position += horizontalMove + forwardMove;
    }
    
}
