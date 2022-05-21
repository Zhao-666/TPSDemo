using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    //移动方向
    private Vector2 _moveDirection;

    //移动速度
    private float _moveSpeed = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + _moveDirection * _moveSpeed * 0.3f);
        }
    }

    public void Init(Vector2 direction)
    {
        _moveDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bound"))
        {
            //子弹碰到墙壁
            Destroy(gameObject);
        }
        else if (other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().Hit();
            Destroy(gameObject);
        }
    }
}