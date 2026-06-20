using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PrototypeMovement : MonoBehaviour
{
    [SerializeField] private float jumpMagnitude;
    private Rigidbody2D _rb;

    private bool _canJump = false;
    private bool _jumpPressed = false;
    private Vector2 _moveDirection;
    [SerializeField] private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_jumpPressed && _canJump)
        {
            //remove previous possible forces
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = 0;
            //jump
            _rb.AddForce(Vector2.up * jumpMagnitude);
            _canJump = false;
        }
        transform.Translate(_moveDirection * (speed * Time.deltaTime));
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //todo: coyote timing
        if (context.performed)
        {
            _jumpPressed = true;
        } else if (context.canceled)
        {
            _jumpPressed = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        _moveDirection.y = 0;
    }
}
