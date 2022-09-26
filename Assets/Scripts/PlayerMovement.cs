using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField][Range(0f, 50f)] private float _gravityFalling;
    [SerializeField][Range(0f, 5f)] private float _jumpHeight;
    [SerializeField][Range(.1f, .5f)] private float _buttonTime = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpCancelRate = 500f;
    [SerializeField] private float _cayoteeTime = .2f;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 10f;

    private CheeseUp _controls;
    private Rigidbody2D _rb;
    private Collider2D _coll;
    private SpriteRenderer _renderer;
    private PlayerAnimation _animation;

    private float _dirX;
    private float _gravityScale;
    private float _jumpForce;
    private float _jumpTime;
    private float _cayoteeCounter;

    private bool _jumping;
    private bool _jumpCancelled;
    private bool _isFacingRight;

    [HideInInspector] public bool IsDead;

    private void Awake()
    {
        _controls = new CheeseUp();
        _controls.Player.Jump.performed += ctx => Jump();
        _controls.Player.Jump.canceled += ctx => _jumpCancelled = true;

        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>(); 
        _renderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<PlayerAnimation>();
        _gravityScale = _rb.gravityScale;
    }

    private void Start()
    {
        _jumpForce = Mathf.Sqrt(-2 * _jumpHeight * Physics2D.gravity.y * _gravityScale);
        _isFacingRight = true;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (IsDead) { return; }

        JumpCancelled();
        ApplyAdditionalGravityOnFalling();
    }

    private void Update()
    {
        if (IsDead) { return; }

        MoveHorizontaly();
        WhileJumping();
        Flip();
        UpdateAnimation();
        UpdateCayoteeTime();
    }

    private void OnMove(InputValue value) => _dirX = value.Get<Vector2>().x;

    private void JumpCancelled()
    {
        if (_jumpCancelled && _jumping && _rb.velocity.y > 0f)
        {
            _rb.AddForce(Vector2.down * _jumpCancelRate);
        }
    }

    private void ApplyAdditionalGravityOnFalling()
    {
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = _gravityFalling;
        }
        else 
        {
            _rb.gravityScale = _gravityScale;
        }
    }

    private void MoveHorizontaly() => _rb.velocity = new Vector2(_dirX * _speed, _rb.velocity.y);

    private void Jump()
    {
        if (_cayoteeCounter > 0f)
        {
            _rb.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            _jumping = true;
            _jumpCancelled = false;
            _jumpTime = 0f;
        }
    }

    private void WhileJumping()
    {
        if (!_jumping) { return; }
        
        _jumpTime += Time.deltaTime;

        if (_jumpTime > _buttonTime)
        {
            _jumping = false;
        } 
    }

    private void Flip()
    {
        if (_dirX > 0f && !_isFacingRight)
        {
            _renderer.flipX = false;
            _isFacingRight = true;
        }
        else if (_dirX < 0f && _isFacingRight)
        {
            _renderer.flipX = true;
            _isFacingRight = false;
        }
    }

    private void UpdateAnimation()
    {
        if (Mathf.Abs(_dirX) > 0.1f)
        {
            _animation.SetAnimationState(PlayerAnimationState.Run);
        }

        if (_rb.velocity.y > 0.1f)
        {
            _animation.SetAnimationState(PlayerAnimationState.Jump);
        }
        else if (_rb.velocity.y < -0.1f)
        {
            _animation.SetAnimationState(PlayerAnimationState.Fall);
        }

        if (_rb.velocity == Vector2.zero)
        {
            _animation.SetAnimationState(PlayerAnimationState.Idle);
        }
    }

    private void UpdateCayoteeTime()
    {
        if (IsGrounded())
        {
            _cayoteeCounter = _cayoteeTime;
        } 
        else
        {
            _cayoteeCounter -= Time.deltaTime;
        }
    }

    private bool IsGrounded() => Physics2D.BoxCast(_coll.bounds.center, _coll.bounds.size, 0f, Vector2.down, .1f, _groundLayer);
}