using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightController : MonoBehaviour, IFightController
{
    [SerializeField] private AnimationController _animationController;
    
    [SerializeField] private FightValues fightValues;

    private bool isInRange = false;

    private IFightController enemyFightController;

    private float healthPoints;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;

        healthPoints = fightValues.healthPoints;
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

        _animationController.TriggerAttackAnimation(PlayerPrefKeys.heavyAttack);

        enemyFightController.TakeDamage(fightValues.heavyAttackDamage);
    }

    public void LightAttack()
    {
        if (!isInRange)
            return;

        _animationController.TriggerAttackAnimation(PlayerPrefKeys.lightAttack);

        enemyFightController.TakeDamage(fightValues.lightAttackDamage);
    }

    public void PlayHeavyAttackVfx()
    {
       
    }

    public void PlayLightAttackVfx()
    {
        
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

            EventSystem.CallPlayerDeath();

            EventSystem.CallGameOver(GameResult.Lose);
        }
    }
}
