using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _finalTimeText;

    private float _currentTime;
    private float _finalTime;
    private bool _countDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = _countDown ? _currentTime -= Time.deltaTime : _currentTime += Time.deltaTime;
        _timerText.text = _currentTime.ToString("0.00");
    }

    private void SetFinalTimeText()
    {
        _finalTime = _currentTime;
        _finalTimeText.text = _finalTime.ToString("0.00");
        GameManager.Instance.SetFinalTimeText("Final time: " + _finalTimeText.text);
    }

    private void OnEnable()
    {
        EndDoor.OnVictoryTriggered += SetFinalTimeText;            // Subscribe to the OnVictoryTriggered event
    }

    private void OnDisable()
    {
        EndDoor.OnVictoryTriggered -= SetFinalTimeText;            // Subscribe to the OnVictoryTriggered event
    }
}
