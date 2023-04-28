using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject SlashEffect;

    public void PlaySlashEffect(Vector2 effectPosition)
    {
        var slashEffect = Instantiate(SlashEffect, new Vector3(effectPosition.x, effectPosition.y, -1), Quaternion.identity);
        slashEffect.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 250), Space.World);
        StartCoroutine(PlayEffect(slashEffect));
    }

    private IEnumerator PlayEffect(GameObject slashEffect)
    {
        yield return new WaitForSeconds(.75f);
        Destroy(slashEffect);

        yield return true;
    }
}
