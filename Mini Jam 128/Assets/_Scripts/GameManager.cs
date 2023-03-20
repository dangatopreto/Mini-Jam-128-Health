using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameObject _gameUIObject;
    [SerializeField] private GameObject _initialScreenObject;
    [SerializeField] private GameObject _victoryScreenObject;
    [SerializeField] private GameObject _gameOverScreenObject;

    [Space]
    [SerializeField] private Text _finalTimeText;

    [Space]
    [SerializeField] private LavaFloor _lavaFloor;

    private void OnEnable()
    {
        EndDoor.OnVictoryTriggered += Victory;            // Subscribe to the OnVictoryTriggered event
    }

    private void OnDisable()
    {
        EndDoor.OnVictoryTriggered -= Victory;            // Subscribe to the OnVictoryTriggered event
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _initialScreenObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void Victory()
    {
        _victoryScreenObject.SetActive(true);
        _gameUIObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        _gameOverScreenObject.SetActive(true);
        _gameUIObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void RestartGame() => SceneManager.LoadScene(0);

    public void StartGame()
    {
        _initialScreenObject.SetActive(false);
        _gameUIObject.SetActive(true);
        _lavaFloor.SetGameStartedBool();
        Time.timeScale = 1;
    }

    public void SetFinalTimeText(string finalTime) => _finalTimeText.text = finalTime;
}
