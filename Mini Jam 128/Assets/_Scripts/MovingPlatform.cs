using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [Header("Moving Platform Attributes")]
    [SerializeField] private float _moveEndPointX;
    [SerializeField] private float _moveDuration;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(_moveEndPointX, _moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
