using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private static readonly Vector3 Positive = Vector3.zero;
    private static readonly Vector3 Negative = new Vector3(0, 180, 0);
    private static readonly int HitAnim = Animator.StringToHash("hit");

    [Header("移动速度"), SerializeField]
    //
    private float moveSpeed = 10f;

    [Header("血量"), SerializeField]
    //
    private float hp = 1f;

    private Transform _transform;
    private Animator _animator;
    private CapsuleCollider2D _collider2D;

    private Transform _playerTransform;

    private bool _startMove;
    private bool _isHit;

    void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CapsuleCollider2D>();

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
        _animator.SetBool(HitAnim, true);
    }

    /// <summary>
    /// Animation call this function.
    /// </summary>
    public void Die()
    {
        if (--hp == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _isHit = false;
            _animator.SetBool(HitAnim, false);
        }
    }

    private void _Move()
    {
        if (!_startMove || _isHit)
        {
            return;
        }

        var heightDiff = Math.Abs(_transform.position.y - _playerTransform.position.y);

        if (heightDiff < 1)
        {
            //在同一平面
            _CheckPlayerDirection();
        }

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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Bound")
            || col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _CheckPlayerDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bound"))
        {
            _collider2D.isTrigger = false;
            _startMove = true;
        }
    }
}