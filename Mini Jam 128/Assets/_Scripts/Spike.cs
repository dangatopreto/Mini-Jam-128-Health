using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spike : MonoBehaviour
{
    private Vector3 _rotation = new Vector3(0, 0, -360);

    [Header("Spike Attributes")]
    [SerializeField] private bool _isMovingType;
    [SerializeField] private float _moveEndPointX;
    [SerializeField] private float _moveDuration;

    private Collider2D _spikeCollider;

    private void Awake()
    {
        _spikeCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(_rotation, 1f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

        if (_isMovingType)
        {
            transform.DOMoveX(_moveEndPointX, _moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.TakeDamage();
            StartCoroutine(TurnOffSpikeColliderRoutine());
        }
    }

    private IEnumerator TurnOffSpikeColliderRoutine()
    {
        _spikeCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _spikeCollider.enabled = true;
    }
}