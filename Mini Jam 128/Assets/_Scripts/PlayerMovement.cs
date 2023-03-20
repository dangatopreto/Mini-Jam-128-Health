using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnPlayerExploded;
    public static event Action OnPlayerLifeAdded;

    private Rigidbody2D _rb;
    //private CatAnimationHandler _anim;
    [SerializeField] private GameObject _playerVisual;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private PlayerAnimationHandler _anim;

    [Space]
    [Header("Stats")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpForce = 20f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;

    [field: Header("Player Attributes")]
    [field: SerializeField] public int currentPlayerLives { get; private set; }     // The current number of the ship's lives
    [field: SerializeField] public int totalPlayerLives { get; private set; }       // The total amount of the ship's lives

    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space]
    [Header("Booleans")]
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _isActive;
    private int _spriteSide = 1;

    [Space]
    [Header("Collision")]
    [SerializeField] private float _collisionRadius = 0.25f;
    [SerializeField] private Vector2 _bottomOffset;

    [Space]
    [Header("Object Pools")]
    [SerializeField] private ObjectPooler _explosionPool;

    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioClip[] _explosionSounds;
    [SerializeField] private AudioClip[] _hurtSounds;

    [Space]
    [Header("Other")]
    [SerializeField] private Transform _explosionSpawnPoint;
    [SerializeField] private ParticleSystem _smokeParticleSystem;

    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<PlayerAnimationHandler>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        currentPlayerLives = totalPlayerLives;
        //_anim = GetComponentInChildren<CatAnimationHandler>();
    }

    private void Start()
    {
        _isActive = true;
        InvokeRepeating("CorrectPlayerSize", 1f, 1f);
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
                _anim.SetHorizontalMovement(x, _rb.velocity.y);

                if (x > 0)
                {
                    _spriteSide = 1;
                    _anim.Flip(_spriteSide);
                }
                if (x < 0)
                {
                    _spriteSide = -1;
                    _anim.Flip(_spriteSide);
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (currentPlayerLives > 0)
                {
                    PlayerJumpLogic();
                }
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

        _anim.SetBool("onGround", _onGround);
    }

    private void PlayerJumpLogic()
    {
        PlayerJump();
        TakeDamage();
        PlayRandomClipFromArray(_explosionSounds);
        _anim.SetTrigger("playerHurt");
        Color32 redColor = new Color32(221, 86, 57, 255);
        StartCoroutine(PlayerChangeColorRoutine(redColor, 0.3f));
        StartCoroutine(SmokeParticlesCoroutine());

        GameObject explosion = _explosionPool.GetObject();
        explosion.transform.position = _explosionSpawnPoint.position;
        explosion.SetActive(true);
        StartCoroutine(DetachExplosionRoutine(explosion));
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
    public void TakeDamage()
    {
        currentPlayerLives--;
        OnPlayerExploded?.Invoke();
        PlayRandomClipFromArray(_hurtSounds);

        if (currentPlayerLives <= 0)
        {
            PlayerDie();
        }
    }

    public void PlayerDie()
    {
        _isActive = false;
        GameManager.Instance.GameOver();
    }

    public void AddHeart()
    {
        if ((currentPlayerLives + 1) > totalPlayerLives)
        {
            totalPlayerLives++;
            currentPlayerLives++;
        }
        else
        {
            currentPlayerLives++;
        }
        OnPlayerLifeAdded?.Invoke();
    }

    private void PlayRandomClipFromArray(AudioClip[] clipArray)
    {
        int randomNumber = UnityEngine.Random.Range(0, clipArray.Length - 1);
        AudioManager.Instance.PlaySoundEffect(clipArray[randomNumber]);
    }

    private IEnumerator DetachExplosionRoutine(GameObject objectToDetach)
    {
        objectToDetach.transform.parent = null;
        yield return new WaitForSeconds(2f);
        objectToDetach.transform.parent = _explosionPool.transform;
        _explosionPool.ReturnObject(objectToDetach);
    }

    private IEnumerator PlayerChangeColorRoutine(Color32 color, float timeToWait)
    {
        _sr.DOColor(color, 0.1f);
        yield return new WaitForSeconds(timeToWait);
        _sr.DOColor(Color.white, 0.3f);
    }

    private IEnumerator SmokeParticlesCoroutine()
    {
        if (_smokeParticleSystem.gameObject.activeSelf == true)
        {
            yield return null;
        }
        else
        {
            _smokeParticleSystem.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            _smokeParticleSystem.Stop();
            yield return new WaitForSeconds(0.7f);
            _smokeParticleSystem.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionRadius);
    }
}
