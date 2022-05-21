using System;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("怪物预制体"), SerializeField]
    //
    private GameObject prefabMonster;

    private Transform _playerTransform;

    private float _createInterval = 2f;
    private float _lastCreateTime;

    private void Awake()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        var currentTime = Time.time;
        if (_lastCreateTime + _createInterval < currentTime)
        {
            _lastCreateTime = currentTime;
            var monster = Instantiate(prefabMonster);
            monster.GetComponent<Monster>().Init(_playerTransform);
        }
    }
}