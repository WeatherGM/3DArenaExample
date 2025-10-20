using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spire : MonoBehaviour
{
    private PlayerEntity _playerEntity;
    private void Start()
    {
        _playerEntity = FindAnyObjectByType<PlayerEntity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Урон");
            _playerEntity.DealDamage(50);
        }
    }
}
