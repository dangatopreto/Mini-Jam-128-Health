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

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(_rotation, 1f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

        if (_isMovingType)
        {
            transform.DOMoveX(_moveEndPointX, _moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
}
