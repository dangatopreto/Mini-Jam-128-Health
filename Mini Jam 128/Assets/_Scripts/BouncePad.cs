using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncePad : MonoBehaviour
{
    [Header("Bounce Pad Attributes")]
    [SerializeField] private float _bounceStrength = 12f;

    [SerializeField] private GameObject _bouncePadVisual;
    private Animator _anim;
    private Collider2D[] _colliders;

    [SerializeField] private AudioClip[] _springSfx;

    private void Awake()
    {
        _anim = _bouncePadVisual.GetComponent<Animator>();
        _colliders = GetComponents<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            playerRb.velocity += Vector2.up * _bounceStrength;

            StartCoroutine(TurnOffColliders(0.2f));
            ActivateBouncePadAnimation();
            PlayRandomBounceSFX();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TurnOffColliders(0.2f));
            ActivateBouncePadAnimation();
            PlayRandomBounceSFX();
        }
    }

    private void ActivateBouncePadAnimation()
    {
        _bouncePadVisual.transform.DOShakeScale(0.2f);
        _anim.SetTrigger("activatePad");
    }

    private void PlayRandomBounceSFX()
    {
        int randomNumber = Random.Range(0, _springSfx.Length - 1);
        AudioManager.Instance.PlaySoundEffect(_springSfx[randomNumber]);
    }

    private IEnumerator TurnOffColliders(float waitTime)
    {
        foreach (Collider2D col in _colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(waitTime);

        foreach (Collider2D col in _colliders)
        {
            col.enabled = true;
        }
    }
}
