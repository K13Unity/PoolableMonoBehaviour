using UnityEngine;
using Utils.FactoryTool;
using System;

public class GroundTile : PoolableMonoBehaviour
{
    public event Action OnPlayerExit;
    public Collider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            OnPlayerExit?.Invoke();
        }
    }
}
