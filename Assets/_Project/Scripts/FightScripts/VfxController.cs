using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VfxController : MonoBehaviour
{
    [SerializeField] private ParticleSystem lightAttackVfx;
    [SerializeField] private ParticleSystem heavyAttackVfx;

    [SerializeField] private ParticleSystem lightDamageVfx;
    [SerializeField] private ParticleSystem heavyDamageVfx;

    [SerializeField] private ParticleSystem stunEffectVfx;

    public void PlayLightAttackVfx()
    {
        lightAttackVfx.Play();
    }
    public void PlayHeavyAttackVfx()
    {
        heavyAttackVfx.Play();
    }

    public void PlayHeavyDamageVfx()
    {
        lightDamageVfx.Play();
    }

    public void PLayLightDamageVfx()
    {
        heavyDamageVfx.Play();
    }

    public void PlayStunEffectVfx(float _duration)
    {
        stunEffectVfx.Play();

        DOVirtual.DelayedCall(_duration, () => stunEffectVfx.Stop());
    }
}
