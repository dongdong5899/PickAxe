using DG.Tweening;
using RPG.Entities;
using System;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;
    [SerializeField] private Entity _entity;

    private void Start()
    {
        _entity.GetCompo<HealthCompo>().OnHealthChangedEvent += HandleHealthChangeEvent;
    }

    private void HandleHealthChangeEvent(int prevValue, int newValue, bool arg3)
    {
        for (int i = newValue; i < prevValue; i++)
        {
            _hearts[i].SetActive(false);
        }
    }
}
