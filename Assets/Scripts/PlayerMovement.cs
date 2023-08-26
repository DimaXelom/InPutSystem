using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    public GameObject[] hearts;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _extraJumpValue;
    [SerializeField] private float _checkRadius;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _health;

    private Vector2 _moveInput;
    private bool _facingRight = true;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private InputAction _jumpAction;

    private float _extraJump;
    private float _speed = 5f;
    private bool _isGround;
   
    private const int MAX_HEALTH = 3;

   

    private enum MovementState
    {
        idle,
        running,
        jumping,
      
    }


    void Start()
    {

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _sprite = _rigidbody2D.GetComponent<SpriteRenderer>();
        _health = MAX_HEALTH;
        _animator = GetComponent<Animator>();

         
    }


    private void Update()
    {
      

        Move();
        Flip();
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (_rigidbody2D.velocity.y > .1f && _isGround == false)
        {
            state = MovementState.jumping;
        }
        

        else
        {
            switch (_moveInput.x)
            {
                case float x when x > 0f:
                    state = MovementState.running;
                    _sprite.flipX = false;
                    break;
                case float x when x < 0f:
                    state = MovementState.running;
                    Flip();
                    break;
                case float x when x == 0f:
                    state = MovementState.idle;
                    break;

                default:
                    state = MovementState.idle;
                    break;
            }
        }

        _animator.SetInteger("state", (int)state);
    }




    private void Move()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(_moveInput.x * _speed, _rigidbody2D.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _isGround = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _whatIsGround);
        if (_isGround == true)
        {
            _extraJump = _extraJumpValue;
        }

        if (context.performed  && _extraJump > 0)
        {
            _rigidbody2D.velocity = Vector2.up * _extraJump;
            _extraJump--;
        }

        else if (context.performed && _extraJump == 0 && _isGround == true)
        {
            _rigidbody2D.velocity = Vector2.up * _jumpForce;
        }


    }

 /*   private async void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.GetComponent<EnemyMovement>())
        {

            _health -= 1;
            UpdateHearts();
        }

        if (_health < 1)
        {
            Destroy(gameObject);
            await Task.Delay(3000);
        }
    }
 */
    private void Flip()
    {
        if (_moveInput.x > 0 && _facingRight == false || _moveInput.x < 0 && _facingRight == true)
        {
            var transform1 = transform;
            Vector3 theScale = transform1.localScale;
            theScale.x *= -1f;
            transform1.localScale = theScale;

            _facingRight = !_facingRight;
           // Debug.Log(_moveInput.x);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _health)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }


}