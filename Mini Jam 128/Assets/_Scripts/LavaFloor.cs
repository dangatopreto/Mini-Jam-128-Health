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
            transform.Translate(0, 0.0025f, 0);
        }
    }

    public void SetGameStartedBool() => _gameHasStarted = true;
}
