using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentHero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hero = HeroRepository.Instance.GetHeroWithUpgradesByName(UserSettingsRepository.Instance.UserData.currentHero);
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(hero.sprite);
    }

}
