using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    private Transform playerTransform;

    [SerializeField] private float minDistanceToPlayer;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 direction;

    private bool isGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag(PlayerPrefKeys.player).transform;
    }

    void FixedUpdate()
    {
        if (!isGameStarted)
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
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
        EventSystem.OnGameOver -= OnGameOver;
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
}
