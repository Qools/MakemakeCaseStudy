using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerInput playerInput;

    private void Update()
    {
        animator.SetFloat(PlayerPrefKeys.direction, playerInput.Velocity);
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStart;
        EventSystem.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStart;
        EventSystem.OnGameOver -= OnGameOver;
    }

    private void OnGameStart()
    {
    }

    private void OnGameOver(GameResult gameResult)
    {
    }
}
