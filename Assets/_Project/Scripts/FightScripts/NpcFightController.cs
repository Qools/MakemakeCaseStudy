using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFightController : MonoBehaviour, IFightController
{
    [SerializeField] private AnimationController _animationController;
    private NpcController npcController;
    
    [SerializeField] private FightValues fightValues;

    private bool isInRange = false;

    private IFightController playerFightController;

    private float healthPoints;

    private float nextAttack = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;

        healthPoints = fightValues.healthPoints;

        npcController = GetComponent<NpcController>();
    }

    private void Update()
    {
        if (!isInRange)
            return;

        if (npcController.isDead)
            return;

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

        _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyAttack);

        playerFightController.TakeDamage(fightValues.heavyAttackDamage);
    }

    public void LightAttack()
    {
        if (!isInRange)
            return;

        _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightAttack);

        playerFightController.TakeDamage(fightValues.lightAttackDamage);
    }

    public void PlayHeavyAttackVfx()
    {
        throw new System.NotImplementedException();
    }

    public void PlayLightAttackVfx()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damageValue)
    {
        healthPoints -= damageValue;

        CheckHealthPoint();

        switch (damageValue)
        {
            case 10f:
                _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightDamage);
                break;
            case 25f:
                _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyDamage);
                break;
        }
    }

    public void CheckHealthPoint()
    {
        if (healthPoints <= 0)
        {
            _animationController.TriggerAttackAnimation(PlayerPrefKeys.death);

            EventSystem.CallNpcDeath();

            EventSystem.CallGameOver(GameResult.Lose);
        }
    }
}
