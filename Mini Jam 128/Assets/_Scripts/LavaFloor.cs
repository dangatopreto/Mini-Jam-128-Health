using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    private bool _gameHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameHasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameHasStarted)
        {
            transform.Translate(0, 0.9f * Time.deltaTime, 0);
        }
    }

    public void SetGameStartedBool(bool value) => _gameHasStarted = value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage();
                player.PlayerDie();
            }
        }
    }
}
