using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFightController 
{
    public void ChooseAttackType();

    public void LightAttack();
    public void HeavyAttack();

    public void PlayLightAttackVfx();
    public void PlayHeavyAttackVfx();

    public void TakeDamage(float damageValue);

    public void CheckHealthPoint();
}