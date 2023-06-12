using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private bool isGameOver;

    public Animator animator
    {
        get { return _animator; }
    }

    private void Start()
    {
        isGameOver = false;

        _animator.SetFloat(PlayerPrefKeys.movementSpeed, 0f);
    }

    private void Update()
    {
        if (isGameOver)
            return;

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
        isGameOver = true;

        _animator.SetFloat(PlayerPrefKeys.movementSpeed, 0f);
    }

    public void TriggerAttackAnimation(string _animationName)
    {
        if (isGameOver)
            return;

        _animator.SetTrigger(_animationName);

        DOVirtual.DelayedCall(1f, ()=> ResetTrigger(_animationName));
    }

    public void ResetTrigger(string _animationName)
    {
        if (isGameOver)
            return;

        _animator.ResetTrigger(_animationName);
    }
}
