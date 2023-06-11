using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private CanvasGroup healthBarCanvasGroup;
    [SerializeField] private Image healthBarImage;

    [SerializeField] private float healthBarChangeDuration;

    private Camera mainCamera;

    private void Start()
    {
        healthBarCanvasGroup.alpha = 0f;

        healthBarImage.fillAmount = 1f;

        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        LookAtToMainCamera();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStarted;
        EventSystem.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
        EventSystem.OnGameOver -= OnGameOver;
    }

    public void OnHealthValueChanged(float _value)
    {
        float newHealthValue = _value * 0.01f;

        healthBarImage.DOFillAmount(newHealthValue, healthBarChangeDuration);
    }

    private void OnGameStarted() 
    {
        healthBarCanvasGroup.alpha = 1f;
    }

    private void OnGameOver(GameResult gameResult)
    {
        healthBarCanvasGroup.alpha = 0f;
    }

    private void LookAtToMainCamera()
    {
        transform.LookAt(mainCamera.transform);
    }
}
