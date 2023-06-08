using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private void Update()
    {
        _animator.SetFloat(PlayerPrefKeys.movementSpeed, _rigidbody.velocity.magnitude);
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
