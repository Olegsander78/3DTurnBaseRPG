using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstance 
{
    public Effect effect;
    public int turnsRemaining;

    public GameObject curActiveGameobject;
    public ParticleSystem curTickParticle;

    public EffectInstance(Effect effect)
    {
        this.effect = effect;
        turnsRemaining = effect.durationOfTurns;
    }
}
