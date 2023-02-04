using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    [SerializeField]
    private Image _energybar;

    [SerializeField]
    private Image _gameOverImg;

    private void Start () {
        _gameOverImg.gameObject.SetActive ( false );
    }

    public void SetEnergyBar(float percentage ) {
        _energybar.fillAmount = Mathf.Clamp01 ( percentage );
    }

    public void ToggleGameOver(bool state ) {
        _gameOverImg.gameObject.SetActive ( state );
    }
}
