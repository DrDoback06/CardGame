using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScaleGroup : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _gameObjects;

    [SerializeField]
    private float _delay = 3;

    [SerializeField]
    private float _additiveScale = .25f;

    [SerializeField]
    private float _speed = .25f;

    [SerializeField]
    private float _delayInBetween = .5f;

    void Start()
    {
        StartCoroutine(PunchScaleGameObjects());
    }

    private IEnumerator PunchScaleGameObjects()
    {
        yield return new WaitForSeconds(_delay);

        foreach (var item in _gameObjects)
        {
            item.transform.DOPunchScale(new Vector3(_additiveScale, _additiveScale, 0), _speed);
            yield return new WaitForSeconds(_delayInBetween);
        }

        StartCoroutine(PunchScaleGameObjects());
    }
}
