using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class FightButton : MonoBehaviour
{
    private CanvasGroup button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<CanvasGroup>();

        DisableButton(GameResult.Win);
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += EnableButton;
        EventSystem.OnGameOver += DisableButton;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= EnableButton;
        EventSystem.OnGameOver -= DisableButton;
    }

    private void EnableButton()
    {
        button.DOFade(1, 0.2f);
        button.interactable = true;
    }

    private void DisableButton(GameResult gameResult)
    {
        button.DOFade(0, 0.2f);
        button.interactable = false;
    }
}
