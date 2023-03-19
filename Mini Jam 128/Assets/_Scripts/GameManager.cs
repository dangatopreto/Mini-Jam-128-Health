using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;   // Initialize the GameManager singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager instance is NULL!");

            return _instance;
        }
    }

    [field: Header("Player Attributes")]
    [field: SerializeField] public int currentPlayerLives { get; private set; }     // The current number of the ship's lives
    [field: SerializeField] public int totalPlayerLives { get; private set; } = 3;  // The total amount of the ship's lives

    private void Awake()
    {
        _instance = this;
        currentPlayerLives = totalPlayerLives;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentPlayerLives--;
    }
}
