using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float minDistanceToPlayer;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 direction;

    public bool isGameStarted = false;

    public bool isDead = false;


    void FixedUpdate()
    {
        if (!isGameStarted)
            return;

        if (isDead) 
            return;

        direction = (playerTransform.position - transform.position).normalized;

        Rotate();

        if (CalculateDistance())
        {
            Stop();

            return;
        }

        else
        {
            _rigidbody.isKinematic = false;
        }

        Movement();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStarted;
        EventSystem.OnGameOver += OnGameOver;
        EventSystem.OnNpcDeath += SetDead;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
        EventSystem.OnGameOver -= OnGameOver;
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
        isGameStarted = false;
        Stop();
    }

    private void Movement()
    {
        Vector3 newVelocity = movementSpeed * Time.fixedDeltaTime * direction;
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;
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

    private void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    private bool CalculateDistance()
    {
        bool isNear = false;

        if (Vector3.Distance(playerTransform.position, transform.position) <= minDistanceToPlayer)
            isNear = true;

        return isNear;
    }

    public void SetDead()
    {
        isDead = true;
    }
}
