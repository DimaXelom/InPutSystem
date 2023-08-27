using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _groundLayer;

    [SerializeField] float _speed;
    [SerializeField] float _jumpPower;

    private float _horizontal;
    private bool isFacingRight = true;




    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(_horizontal * _speed, _rigidbody2D.velocity.y);
        if (!isFacingRight && _horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && _horizontal < 0f)
        {
            Flip();
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGround())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpPower);
        }
        if (context.canceled && _rigidbody2D.velocity.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.5f);
        }
    }
    private bool IsGround()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;

    }

}
