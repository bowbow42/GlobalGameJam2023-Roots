using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour {

    [SerializeField]
    private Image _energybar;

    [SerializeField]
    private Image _gameOverImg;

    [SerializeField]
    private TextMeshProUGUI _scroreText;

    private void Start () {
        _gameOverImg.gameObject.SetActive ( false );
        SetScore ( 0 );
    }

    public void SetEnergyBar(float percentage ) {
        _energybar.fillAmount = Mathf.Clamp01 ( percentage );
    }

    public void SetScore(int score ) {
        _scroreText.text = score.ToString ();
    }

    public void ToggleGameOver(bool state ) {
        _gameOverImg.gameObject.SetActive ( state );
    }
}
