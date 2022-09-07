using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAction : ScriptableObject
{
    public string displayName;
    public string discription;

    public abstract void Cast(Character caster, Character target);
}
