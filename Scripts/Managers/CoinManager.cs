using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    private int Coins;
    [SerializeField]
    private TextMeshProUGUI _coinText;

    private void Awake()
    {
        Coins = 0;
    }

    public void AddCoins(int value)
    {
        Coins += value;

        UpdateCoinText(Coins, Color.yellow);
    }

    /// <summary>
    /// Used to modify the coins in the user settings
    /// Don't use this in game, only in menus
    /// </summary>
    /// <param name="coinValue"></param>
    public void ModifyUserCoins(int coinValue)
    {
        UserSettingsRepository.Instance.UserData.coins += coinValue;
        UserSettingsRepository.Instance.Save();

        UpdateCoinText(UserSettingsRepository.Instance.UserData.coins, coinValue < 0 ? Color.red : Color.yellow);
    }

    private void UpdateCoinText(int value, Color color)
    {
        if (_coinText != null)
        {
            _coinText.text = value.ToString();

            var origScale = _coinText.transform.localScale.x;
            _coinText.DOColor(color, .1f).OnComplete(() => _coinText.DOColor(Color.white, .1f));
            _coinText.transform.DOScale(origScale * 1.25f, .1f).OnComplete(() => _coinText.transform.DOScale(origScale, .1f));
        }
    }

    /// <summary>
    /// Adds the current coins onto the UserData coins.
    /// Updates coins last round to show in game over screen
    /// Should only be used when the game is over.
    /// </summary>
    public void SaveCoins()
    {
        // Validation to prevent overriding coins last round
        if(Coins != 0)
        {
            UserSettingsRepository.Instance.UserData.coinsLastRound = (0 + Coins);
            UserSettingsRepository.Instance.UserData.coins += Coins;
            Coins = 0;
        }
    }

    public int GetUserDataCoins()
    {
        return UserSettingsRepository.Instance.UserData.coins;
    }

    public int GetCurrentCoins()
    {
        return Coins;
    }

    public int GetCoinsLastRound()
    {
        return UserSettingsRepository.Instance.UserData.coinsLastRound;
    }

}
