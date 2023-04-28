using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUiInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject _coinTextGameObject;
    private TextMeshProUGUI _coinText;

    public bool IsGlobalCoinAmount = false;
    public bool IsLastRoundCoinAmount = false;

    private void Start()
    {
        _coinText = _coinTextGameObject.GetComponent<TextMeshProUGUI>();

        if (IsGlobalCoinAmount) { _coinText.text = UserSettingsRepository.Instance.UserData.coins.ToString(); }
        else if (IsLastRoundCoinAmount) { _coinText.text = CoinManager.Instance.GetCoinsLastRound().ToString(); }
    }

}
