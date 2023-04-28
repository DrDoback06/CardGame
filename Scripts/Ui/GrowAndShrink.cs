using UnityEngine;
using DG.Tweening;
using System;

public class GrowAndShrink : MonoBehaviour
{

    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _scaleCeiling = 1.1f;
    [SerializeField]
    private float _scaleFloor = .9f;

    private float _initialScale = 1;

    private void Awake()
    {
        _initialScale = gameObject.transform.localScale.x;

        StartScaling(gameObject);
    }

    void StartScaling(GameObject gameObject)
    {
        ScaleUp(gameObject).OnComplete(() => ScaleDown(gameObject).OnComplete(() => StartScaling(gameObject)));
    }

    private Tweener ScaleDown(GameObject gameObject)
    {
        return gameObject.transform.DOScale(_initialScale * _scaleFloor, 1 * _speed);
    }

    private Tweener ScaleUp(GameObject gameObject)
    {
        return gameObject.transform.DOScale(_initialScale * _scaleCeiling, 1 * _speed);
    }
}
