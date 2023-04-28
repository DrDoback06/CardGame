using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HeroSelectManager : MonoBehaviour
{
    private int _currentPrecedence = 0;

    [SerializeField]
    private TextMeshProUGUI _heroDescription;
    [SerializeField]
    private TextMeshProUGUI _heroName;
    [SerializeField]
    private TextMeshProUGUI _heroHealth;
    [SerializeField]
    private Image _heroImage;

    private int _upgradeCost;
    [SerializeField]
    private TextMeshProUGUI _upgradeCostText;

    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        InitHeroSelect(HeroRepository.Instance.GetHeroWithUpgradesByPrecedence(_currentPrecedence));
    }

    /// <summary>
    /// Initialises the hero the user can currently see.
    /// Call this with other heroes to switch.
    /// </summary>
    /// <param name="heroData"></param>
    private void InitHeroSelect(HeroData heroData)
    {
        _heroDescription.text = heroData.description;
        _heroName.text = heroData.name;
        _heroHealth.text = heroData.health.ToString();
        _heroImage.sprite = Resources.Load<Sprite>(heroData.sprite);
        _upgradeCost = heroData.nextUpgradeCost;
        _upgradeCostText.text = _upgradeCost.ToString();
    }

    /// <summary>
    /// Increments the current hero (if possible)
    /// Then switches the hero.
    /// </summary>
    public void Next()
    {
        if (_currentPrecedence != HeroRepository.Instance.GetMaxPrecidence())
        {
            _currentPrecedence++;
            StartCoroutine(ChangeHero());
        }
    }

    /// <summary>
    /// decrements the current hero (if possible)
    /// Then switches the hero.
    /// </summary>
    public void Previous()
    {
        if (_currentPrecedence != 0)
        {
            _currentPrecedence--;
            StartCoroutine(ChangeHero());
        }
    }

    /// <summary>
    /// Selects the current hero as the user's chosen hero.
    /// Chosen hero will be used when the game starts
    /// </summary>
    public void Select()
    {
        UserSettingsRepository.Instance.UserData.currentHero = HeroRepository.Instance.GetHeroWithUpgradesByPrecedence(_currentPrecedence).name;
        UserSettingsRepository.Instance.Save();
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, .25f);
        yield return new WaitForSeconds(.30f);
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Animation to change the hero.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeHero()
    {
        _leftButton.enabled = false;
        _rightButton.enabled = false;

        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, .25f);
        yield return new WaitForSeconds(.30f);
        InitHeroSelect(HeroRepository.Instance.GetHeroWithUpgradesByPrecedence(_currentPrecedence));
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, .25f);
        yield return new WaitForSeconds(.25f);

        _leftButton.enabled = true;
        _rightButton.enabled = true;
    }

    /// <summary>
    /// Upgrades the current hero if possible.
    /// </summary>
    public void UpgradeHero()
    {
        if (CoinManager.Instance.GetUserDataCoins() >= _upgradeCost)
        {
            CoinManager.Instance.ModifyUserCoins(-_upgradeCost);
            UserSettingsRepository.Instance.Upgrade(_heroName.text);

            InitHeroSelect(HeroRepository.Instance.GetHeroWithUpgradesByPrecedence(_currentPrecedence));
        }
    }
}
