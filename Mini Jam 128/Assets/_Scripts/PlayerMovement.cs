using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnPlayerExploded;

    private Rigidbody2D _rb;
    //private CatAnimationHandler _anim;
    [SerializeField] private GameObject _playerVisual;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Collider2D playerCollider;

    [Space]
    [Header("Stats")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpForce = 20f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;

    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space]
    [Header("Booleans")]
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _isActive;

    [Space]
    [Header("Collision")]
    [SerializeField] private float _collisionRadius = 0.25f;
    [SerializeField] private Vector2 _bottomOffset;

    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_anim = GetComponentInChildren<CatAnimationHandler>();
    }

    private void Start()
    {
        _isActive = true;
        InvokeRepeating("CorrectPlayerSize", 1f, 1f);
        //_anim.SetBool("isActive", true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isActive)
        {
            if (_canMove)
            {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                Vector2 dir = new Vector2(x, y);

                PlayerRun(dir);
                //_anim.SetHorizontalMovement(x, _rb.velocity.y);
            }

            if (Input.GetButtonDown("Jump"))
            {
                /*if (_onGround)
                {*/
                    //_anim.SetTrigger("jump");
                    PlayerJump();
                    OnPlayerExploded?.Invoke();
                //}
            }

            if (Input.GetButtonDown("Fire1") && _onGround)
            {
                //_anim.SetTrigger("attack");
                // Attack Logic
            }
            if (Input.GetButtonDown("Fire2"))
            {
                // Attack Logic
            }
        }
        else if (!_isActive)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                // Attack Logic
            }
        }

        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }

        _onGround = Physics2D.OverlapCircle((Vector2)transform.position + _bottomOffset, _collisionRadius, _groundLayer);

        if (_onGround && _playerVisual.transform.eulerAngles != Vector3.zero)
        {
            _playerVisual.transform.rotation = Quaternion.identity;
        }

        //_anim.SetBool("onGround", _onGround);
    }

    private void CorrectPlayerSize() => _playerVisual.transform.localScale = Vector3.one;

    private void PlayerRun(Vector2 dir)
    {
        if (!_canMove)
            return;

        _rb.velocity = new Vector2(dir.x * _moveSpeed, _rb.velocity.y);
    }

    private void PlayerJump()
    {
        _playerVisual.transform.DOShakeScale(0.6f, 1.1f).SetEase(Ease.OutSine);
        float playerRotationValue = UnityEngine.Random.Range(-180f, 180f);
        Vector3 targetPlayerRotation = new Vector3(0f, 0f, playerRotationValue * 5f);
        _playerVisual.transform.DOLocalRotate(targetPlayerRotation, 1.2f).SetEase(Ease.OutSine);
        //_playerVisual.transform.DOPunchRotation(targetPlayerRotation, 0.8f).SetEase(Ease.OutSine);

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += Vector2.up * _jumpForce;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionRadius);
    }
}
