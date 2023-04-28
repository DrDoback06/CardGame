using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject incentivizedAdText;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void GoToHeroSelect()
    {
        SceneManager.LoadScene("HeroSelect", LoadSceneMode.Single);
    }
    
    /// <summary>
    /// Make sure you activate Unity Ads for your project
    /// </summary>
    public void InitiateCoinIncentiveAd()
    {
        // Code to run ad
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                CoinManager.Instance.AddCoins(CoinManager.Instance.GetCoinsLastRound());
                incentivizedAdText.GetComponent<TextMeshProUGUI>().text = "Coins added to your account!";
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
