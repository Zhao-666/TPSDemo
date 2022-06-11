using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("怪物预制体"), SerializeField]
    //
    private GameObject prefabMonster;

    [Header("大怪物预制体"), SerializeField]
    //
    private GameObject prefabBoss;

    private Transform _playerTransform;

    private float _createInterval = 2f;
    private float _lastCreateTime;

    private int _monsterLimit = 10;
    private int _currentMonsterCount;

    private bool _hasBoss;

    private void Awake()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        var currentTime = Time.time;
        if (_lastCreateTime + _createInterval < currentTime
            && _monsterLimit > _currentMonsterCount)
        {
            _currentMonsterCount++;
            _lastCreateTime = currentTime;
            var monster = Instantiate(prefabMonster);
            monster.GetComponent<Monster>().Init(_playerTransform);
        }

        if (!_hasBoss && _playerTransform.position.x > 28)
        {
            _hasBoss = true;
            var monster = Instantiate(prefabBoss);
            monster.GetComponent<Monster>().Init(_playerTransform);
        }
    }
}