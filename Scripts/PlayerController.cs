using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly Vector3 Positive = Vector3.zero;
    private static readonly Vector3 Negative = new Vector3(0, 180, 0);

    [Header("移动速度"), SerializeField]
    //
    private float moveSpeed = 10f;

    [Header("跳跃力度"), SerializeField]
    //
    private float jumpForce = 8f;

    [Header("下降重力"), SerializeField]
    //
    private float fallMultiplier = 5f;
    
    [Header("小跳重力"), SerializeField]
    //
    private float lowJumpMultiplier = 3f;

    private Transform _transform;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private float _inputX;
    private bool _inputJump;
    private bool _moving;

    void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _inputX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            _inputJump = true;
        }

        _Move();
    }

    private void FixedUpdate()
    {
        _Jump();
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

        if (_inputJump)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _inputJump = false;
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
                _transform.eulerAngles = _inputX > 0 ? Positive : Negative;
            }
        }
        else if (_moving)
        {
            _moving = false;
            _animator.SetBool(Run, false);
        }
    }
}