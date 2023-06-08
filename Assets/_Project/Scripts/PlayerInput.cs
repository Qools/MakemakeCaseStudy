using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 direction;

    private bool isGameStarted = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGameStarted)
            return;

        direction = new Vector3(Joystick.current.GetAxis("Horizontal"), 0f, Joystick.current.GetAxis("Vertical"));

        Rotate();

        Movement();
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

    private void OnGameStarted()
    {
        isGameStarted = true;
    }

    private void OnGameOver(GameResult gameResult)
    {
        Stop();
    }

    private void Movement()
    {
        Vector3 newVelocity = Joystick.current.knobDistance * movementSpeed * Time.fixedDeltaTime * direction;
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;
    }

    private void Rotate()
    {
        character.rotation = Quaternion.RotateTowards(
            character.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.fixedDeltaTime);
    }

    private void Stop()
    {
        //_animator.ResetTrigger(PlayerPrefKeys.runTrigger);
        //_animator.SetTrigger(PlayerPrefKeys.stopTrigger);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }
}
