using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
    [SerializeField] private AudioClip[] _heartCollectableSfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.AddHeart();
            PlayRandomHeartCollectedSFX();
            Destroy(this.gameObject);
        }
    }

    private void PlayRandomHeartCollectedSFX()
    {
        int randomNumber = Random.Range(0, _heartCollectableSfx.Length - 1);
        AudioManager.Instance.PlaySoundEffect(_heartCollectableSfx[randomNumber]);
    }
}
