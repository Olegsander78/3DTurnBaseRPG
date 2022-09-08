using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    [Header("Stats")]
    public Team team;
    public string displayName;
    public int curHp;
    public int maxHp;

    [Header("Combat Actions")]
    public CombatAction[] combatActions;

    [Header("Components")]
    public CharacterEffects characterEffects;
    public CharacterUI characterUI;
    public GameObject selectionVisual;
    public DamageFlash damageFlash;

    [Header("Prefabs")]
    public GameObject healParticalPrefab;

    public Vector3 standingPosition;

    private void OnEnable()
    {
        TurnManager.instance.onNewTurn += OnNewTurn;
    }
    private void OnDisable()
    {
        TurnManager.instance.onNewTurn -= OnNewTurn;
    }

    void OnNewTurn()
    {
        characterUI.ToggleTurnVisual(TurnManager.instance.GetCurrentTurnCharacter() == this); ;
    }
    public void CastCombatAction(CombatAction combatAction, Character target = null)
    {

    }
    public void TakeDamage(int damage)
    {

    }
    public void Heal(int amount)
    {

    }
    public void Die()
    {

    }
    public void MoTotarget(Character target, UnityAction<Character> arriveCallback)
    {

    }

    public void ToggleSelectionVisual(bool toggle)
    {
        selectionVisual.SetActive(toggle);
    }
}
