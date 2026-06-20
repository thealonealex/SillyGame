using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PrototypeMovement : MonoBehaviour
{
    [SerializeField] private float jumpMagnitude;
    private Rigidbody2D _rb;

    private string _currentlyTouching;
    private bool _jumpPressed = false;
    private Vector2 _moveDirection;
    [SerializeField] private float speed;
    private Vector2 _walljumpDirection;

    [SerializeField] private float coyoteTime = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_jumpPressed && _currentlyTouching == "Ground")
        {
            //remove previous possible forces
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = 0;
            //jump
            _rb.AddForce(Vector2.up * jumpMagnitude);
            _jumpPressed = false;
        }if (_jumpPressed && _currentlyTouching == "Wall")
        {
            //do a backjolt
            _walljumpDirection = -_moveDirection;
            StartCoroutine(Walljump());
            _rb.AddForce(Vector2.up * jumpMagnitude);
            _jumpPressed = false;
        }

        if (_walljumpDirection != Vector2.zero)
        {
            _walljumpDirection = _walljumpDirection + _moveDirection * (Time.deltaTime * 5);
        }
        _rb.linearVelocity = _walljumpDirection != Vector2.zero && _moveDirection != Vector2.zero ? new Vector2(_walljumpDirection.x * speed, _rb.linearVelocity.y) : new Vector2(_moveDirection.x * speed, _rb.linearVelocity.y);
    }

    private IEnumerator Walljump()
    {
        yield return new WaitForSeconds(0.2f);
        _walljumpDirection = Vector2.zero;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        //todo: coyote timing
        //todo: extra force during walljumping
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
        if (_currentlyTouching == "Ground" && collision.gameObject.CompareTag("Wall")) return;
        _currentlyTouching = collision.gameObject.tag;
        Debug.Log(_currentlyTouching);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_currentlyTouching))
        {
            StartCoroutine(CoyoteTime("Nothing"));
        }
        Debug.Log(_currentlyTouching);
    }

    private IEnumerator CoyoteTime(string Tag)
    {
        yield return new WaitForSeconds(coyoteTime);
        _currentlyTouching = Tag;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        _moveDirection.y = 0;
    }
}
