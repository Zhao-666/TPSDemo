using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _transform;
    private Transform _playerTransform;
    private Player _player;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        _playerTransform = GameObject.Find("Player").transform;
        _player = _playerTransform.GetComponent<Player>();
    }

    void Update()
    {
        var currentPos = _transform.position;
        var playerPosX = _playerTransform.position.x;
        if (Math.Abs(playerPosX - currentPos.x) > 0.1f && !_player.IsStickToWall())
        {
            if (playerPosX > 0 && playerPosX < 37)
            {
                _transform.position = new Vector3(playerPosX, currentPos.y, currentPos.z);
            }
        }
    }
}