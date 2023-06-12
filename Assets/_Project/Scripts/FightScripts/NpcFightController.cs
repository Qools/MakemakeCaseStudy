using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFightController : MonoBehaviour, IFightController
{
    [SerializeField] private AnimationController _animationController;
    private NpcController npcController;
    [SerializeField] private HealthBarView healthBarView;
    [SerializeField] private VfxController vfxController;

    [SerializeField] private FightValues fightValues;

    private bool isInRange = false;

    private IFightController playerFightController;

    private float healthPoints;

    private float nextAttack = 0.15f;

    private bool isStunned = false;
    private float stunTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;
        isStunned = false;

        healthPoints = fightValues.healthPoints;

        npcController = GetComponent<NpcController>();
    }

    private void Update()
    {
        if (!npcController.isGameStarted)
            return;

        if (!isInRange)
            return;

        if (npcController.isDead)
            return;

        if (isStunned)
        {
            StunTimer();
            return;
        }

        if (Time.time > nextAttack)
        {
            ChooseAttackType();
            nextAttack = Time.time + fightValues.attackCooldown; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.player))
        {
            isInRange = true;

            playerFightController = other.GetComponent<IFightController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.player))
        {
            isInRange = false;
        }
    }

    public void ChooseAttackType()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < fightValues.heavyAttackChange)
        {
            HeavyAttack();
        }

        else
        {
            LightAttack();
        }
    }

    public void HeavyAttack()
    {
        if (!isInRange)
            return;

        if (_animationController.animator.GetFloat(PlayerPrefKeys.movementSpeed) > 0.1f)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.runAttack);
        }

        else
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyAttack);
        }
       
        playerFightController.TakeDamage(fightValues.heavyAttackDamage);

        vfxController.PlayHeavyAttackVfx();
    }

    public void LightAttack()
    {
        if (!isInRange)
            return;

        if (_animationController.animator.GetFloat(PlayerPrefKeys.movementSpeed) > 0.1f)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.runAttack);
        }

        else
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightAttack);
        }

        playerFightController.TakeDamage(fightValues.lightAttackDamage);

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

            EventSystem.CallNpcDeath();

            EventSystem.CallGameOver(GameResult.Win);
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
