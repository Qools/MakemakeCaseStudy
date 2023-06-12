using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightController : MonoBehaviour, IFightController
{
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private HealthBarView healthBarView;
    [SerializeField] private VfxController vfxController;
    [SerializeField] private FightValues fightValues;

    private bool isInRange = false;

    private IFightController enemyFightController;

    private float healthPoints;

    private bool isStunned = false;
    private float stunTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;

        healthPoints = fightValues.healthPoints;
    }

    private void Update()
    {
        if (isStunned)
        {
            StunTimer();
            return;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            LightAttack();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            HeavyAttack();
        }
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.enemy))
        {
            isInRange = true;

            enemyFightController = other.GetComponent<IFightController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.enemy))
        {
            isInRange = false;
        }
    }

    public void ChooseAttackType()
    {
        
    }

    public void HeavyAttack()
    {
        if (!isInRange)
            return;

        if (isStunned)
            return;

        if (_animationController.animator.GetFloat(PlayerPrefKeys.movementSpeed) > 0.1f)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.runAttack);
        }

        else
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyAttack);
        }

        enemyFightController.TakeDamage(fightValues.heavyAttackDamage);

        vfxController.PlayHeavyAttackVfx();
    }

    public void LightAttack()
    {
        if (!isInRange)
            return;

        if (isStunned)
            return;

        if (_animationController.animator.GetFloat(PlayerPrefKeys.movementSpeed) > 0.1f)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.runAttack);
        }

        else
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightAttack);
        }

        enemyFightController.TakeDamage(fightValues.lightAttackDamage);

        vfxController.PlayLightAttackVfx();
    }

    public void TakeDamage(float damageValue)
    {
        healthPoints -= damageValue;

        healthBarView.OnHealthValueChanged(healthPoints);

        CheckHealthPoint();

        switch (damageValue)
        {
            case 10f:
                _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightDamage);
                vfxController.PLayLightDamageVfx();
                break;
            case 25f:
                _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyDamage);
                vfxController.PlayHeavyDamageVfx();
                CalculateStunChance();
                break;
        }
    }

    public void CheckHealthPoint()
    {
        if (healthPoints <= 0)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.death);

            EventSystem.CallPlayerDeath();

            EventSystem.CallGameOver(GameResult.Lose);
        }
    }

    private void CalculateStunChance()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < fightValues.stunChance)
        {
            isStunned = true;

            vfxController.PlayStunEffectVfx(fightValues.stunDuration);
        }

        else
        {
            isStunned = false;
        }
    }

    private void StunTimer()
    {
        if (stunTimer < fightValues.stunDuration)
        {
            stunTimer += Time.time;
        }

        else
        {
            stunTimer = 0f;
            isStunned = false;
        }
    }
}
