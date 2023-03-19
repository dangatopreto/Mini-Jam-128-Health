using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeUI : MonoBehaviour
{
    [Header("Life UI Variables")]
    [SerializeField] private GameObject _lifeIconPrefab;    // The prefab of the life icon
    [SerializeField] private PlayerMovement _playerHealth;      // The player's health value
    [SerializeField] private List<PlayerLives> _lives = new List<PlayerLives>();    // A list of the player's lives

    private void OnEnable()
    {
        PlayerMovement.OnPlayerExploded += DrawLives;            // Subscribe to the OnPlayerDamaged event
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerExploded -= DrawLives;            // Unsubscribe to the OnPlayerDamaged event
    }

    public void DrawLives()                                 // Draw as many life icons as necessary
    {
        ClearLives();
        int livesToMake = _playerHealth.totalPlayerLives;
        for (int i = 0; i < livesToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < _lives.Count; i++)
        {
            if (i <= _playerHealth.currentPlayerLives - 1)
            {
                _lives[i].SetLifeImage(LivesStatus.Full);
            }
        }
    }

    public void CreateEmptyHeart()                          // Instantiates the lives' icons
    {
        GameObject newLife = Instantiate(_lifeIconPrefab);
        newLife.transform.SetParent(transform);

        RectTransform rectTransform = newLife.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        PlayerLives lifeComponent = newLife.GetComponent<PlayerLives>();
        lifeComponent.SetLifeImage(LivesStatus.Empty);
        _lives.Add(lifeComponent);
    }

    public void ClearLives()                                // Removes all lives' icons
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        _lives = new List<PlayerLives>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawLives();
    }
}