using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly Vector3 Positive = Vector3.zero;
    private static readonly Vector3 Negative = new Vector3(0, 180, 0);

    [Header("移动速度"), SerializeField]
    //
    private float moveSpeed = 10f;

    [Header("跳跃力度"), SerializeField]
    //
    private float jumpForce = 5f;

    [Header("下降重力"), SerializeField]
    //
    private float fallMultiplier = 2.5f;

    [Header("小跳重力"), SerializeField]
    //
    private float lowJumpMultiplier = 2f;

    [Header("地面Layer"), SerializeField]
    //
    private LayerMask groundLayerMask;

    [Header("子弹预制体"), SerializeField]
    //
    private GameObject prefabBullet;
    
    [Header("音频组件")]
    //
    public AudioSource mainAudioSource;

    [Header("音频")]
    //
    public AudioClip jumpClip;

    public AudioClip shootClip;

    private Transform _transform;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private float _inputX;
    private bool _inputJump;
    private bool _moving;
    private bool _isGrounded;
    private Vector2 _forwardDirection = Vector2.right;
    private Vector2 _playerSize;

    //地面检测盒子
    private float _checkBoxHeight = 0.2f;
    private Vector2 _checkBoxSize;

    //射击相关
    private float _fireInterval = 0.3f;
    private float _lastFireTime;

    void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _playerSize = _spriteRenderer.bounds.size;
        _checkBoxSize = new Vector2(_playerSize.x * 0.8f, _checkBoxHeight);
    }

    void Update()
    {
        _inputX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            _inputJump = true;
        }

        if (Input.GetButton("Fire1"))
        {
            _Fire();
        }

        _Move();
    }

    private void FixedUpdate()
    {
        _Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        var checkBoxCenter = (Vector2) _transform.position - (Vector2.up * _playerSize.y * 0.5f);
        Gizmos.DrawWireCube(checkBoxCenter, _checkBoxSize);
    }

    private void _Fire()
    {
        var currentTime = Time.time;
        if (_lastFireTime + _fireInterval < currentTime)
        {
            _lastFireTime = currentTime;
            var bullet = Instantiate(prefabBullet, _transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Init(_forwardDirection);
            
            mainAudioSource.clip = shootClip;
            mainAudioSource.Play();
        }
    }

    private void _Jump()
    {
        if (_rigidbody2D.velocity.y < 0)
        {
            //下降中
            _rigidbody2D.gravityScale = fallMultiplier;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //按了一下就立刻放开
            _rigidbody2D.gravityScale = lowJumpMultiplier;
        }
        else
        {
            _rigidbody2D.gravityScale = 1f;
        }

        if (_inputJump && _isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _inputJump = false;
            _isGrounded = false;
            
            mainAudioSource.clip = jumpClip;
            mainAudioSource.Play();
        }
        else
        {
            _CheckPlayerIsGrounded();
        }
    }

    private void _CheckPlayerIsGrounded()
    {
        var checkBoxCenter = (Vector2) _transform.position - (Vector2.up * _playerSize.y * 0.5f);
        if (Physics2D.OverlapBox(checkBoxCenter, _checkBoxSize, 0, groundLayerMask))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void _Move()
    {
        var moveX = Math.Abs(_inputX);
        if (moveX > 0.001f)
        {
            _transform.Translate(new Vector3(moveX * moveSpeed * Time.deltaTime, 0, 0));
            if (!_moving)
            {
                _moving = true;
                _animator.SetBool(Run, true);
                if (_inputX > 0)
                {
                    _transform.eulerAngles = Positive;
                    _forwardDirection = Vector2.right;
                }
                else
                {
                    _transform.eulerAngles = Negative;
                    _forwardDirection = Vector2.left;
                }
            }
        }
        else if (_moving)
        {
            _moving = false;
            _animator.SetBool(Run, false);
        }
    }
}