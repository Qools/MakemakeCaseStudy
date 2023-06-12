using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Joystick joystick;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 direction;

    private bool isGameStarted = false;

    private bool isDead = false;

    public float rigidbodyVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGameStarted)
            return;

        if (isDead)
            return;

        direction = new Vector3(
            joystick.Horizontal,
            0f,
            joystick.Vertical);

        Rotate();

        Movement();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStarted;
        EventSystem.OnGameOver += OnGameOver;
        EventSystem.OnJoystickButtonUp += OnJoystickButtonUp;
        EventSystem.OnNpcDeath += SetDead;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
        EventSystem.OnGameOver -= OnGameOver;
        EventSystem.OnJoystickButtonUp -= OnJoystickButtonUp;
        EventSystem.OnNpcDeath -= SetDead;
    }

    private void OnGameStarted()
    {
        isGameStarted = true;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = false;
    }

    private void OnGameOver(GameResult gameResult)
    {
        Stop();
    }

    private void Movement()
    {
        Vector3 newVelocity = joystick.HandleRange * movementSpeed * Time.fixedDeltaTime * direction;
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;

        rigidbodyVelocity = _rigidbody.velocity.magnitude;
    }

    private void Rotate()
    {
        if (direction == Vector3.zero)
            return;

        character.rotation = Quaternion.RotateTowards(
            character.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnJoystickButtonUp()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void SetDead()
    {
        isDead = true;

        Stop();
    }
}
