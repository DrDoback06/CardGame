using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreenInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject coinsLastRoundText;
    [SerializeField]
    private GameObject incentivizedCoinsText;

    // Start is called before the first frame update
    void Start()
    {
        var coinsLastRound = CoinManager.Instance.GetCoinsLastRound().ToString();
        coinsLastRoundText.GetComponent<TextMeshProUGUI>().text = coinsLastRound;
        incentivizedCoinsText.GetComponent<TextMeshProUGUI>().text = coinsLastRound;
    }

}
