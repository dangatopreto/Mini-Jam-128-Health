using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    [Header("Life Variables")]
    [SerializeField] private Sprite _fullLife;      // The sprites that indicate the player's lives
    [SerializeField] private Sprite _emptyLife;     // The sprites that indicate the player's lives

    [Header("Component References")]
    [SerializeField] private Image _livesImage;     // The image component

    private void Awake()
    {
        _livesImage = GetComponent<Image>();
    }

    public void SetLifeImage(LivesStatus status)    // Set the life image as either full or empty
    {
        switch (status)
        {
            case LivesStatus.Empty:
                _livesImage.sprite = _emptyLife;
                break;
            case LivesStatus.Full:
                _livesImage.sprite = _fullLife;
                break;
            default:
                Debug.LogError("Couldn't load the life image");
                break;
        }
    }
}
public enum LivesStatus                             // An enum that contains all the possible lives status
    {
        Empty = 0,
        Full = 1,
    }
