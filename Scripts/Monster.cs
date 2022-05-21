using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private static readonly Vector3 Positive = Vector3.zero;
    private static readonly Vector3 Negative = new Vector3(0, 180, 0);
    private static readonly int HitAnim = Animator.StringToHash("Hit");

    [Header("移动速度"), SerializeField]
    //
    private float moveSpeed = 10f;

    private Transform _transform;
    private Animator _animator;
    private CapsuleCollider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private Transform _playerTransform;

    private bool _isHit;

    void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CapsuleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _collider2D.isTrigger = true;
    }

    void Update()
    {
        _Move();
    }

    public void Init(Transform playerTransform)
    {
        _playerTransform = playerTransform;

        _transform.position = new Vector3(_playerTransform.position.x, 15, 0);
    }

    public void Hit()
    {
        _isHit = true;
        _animator.SetTrigger(HitAnim);
    }

    /// <summary>
    /// Animation call this function.
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }

    private void _Move()
    {
        if (_isHit)
        {
            return;
        }
        _CheckPlayerDirection();
        _transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }

    private void _CheckPlayerDirection()
    {
        if (_playerTransform.position.x > _transform.position.x)
        {
            _transform.eulerAngles = Positive;
        }
        else
        {
            _transform.eulerAngles = Negative;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bound"))
        {
            _collider2D.isTrigger = false;
        }
    }
}