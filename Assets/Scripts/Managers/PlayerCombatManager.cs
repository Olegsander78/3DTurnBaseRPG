using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
    public float selectionCheckRate = 0.02f;
    private float lastSelectionCheckTime;
    public LayerMask selectionLayerMask;

    private bool isActive;

    private CombatAction curSelectionCombatAction;
    private Character curSelectedCharacter;

    //Flags
    private bool canSelectSelf;
    private bool canSelectTeam;
    private bool canSelectEnemies;

    public static PlayerCombatManager instance;

    [Header("Components")]
    public CombatActionsUI combatActionsUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

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
        if(TurnManager.instance.GetCurrentTurnCharacter().team == Character.Team.Player)
        {
            EnablePlayerCombat();
        }
        else
        {
            DisablePlayerCombat();
        }
    }
    void EnablePlayerCombat()
    {
        curSelectedCharacter = null;
        curSelectionCombatAction = null;
        isActive = true;

    }
    void DisablePlayerCombat()
    {
        isActive = false;
    }
    private void Update()
    {
        if (!isActive || curSelectionCombatAction == null)
            return;

        if(Time.time - lastSelectionCheckTime > selectionCheckRate)
        {
            lastSelectionCheckTime = Time.time;
            SelectionCheck();
        }

        if (Mouse.current.leftButton.isPressed && curSelectedCharacter != null)
        {
            CastCombatAction();
        }
    }
    void SelectionCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 999, selectionLayerMask))
        {
            Character character = hit.collider.GetComponent<Character>();

            if (curSelectedCharacter != null && curSelectedCharacter == character)
                return;

            if (canSelectSelf && character == TurnManager.instance.GetCurrentTurnCharacter())
            {
                SelectCharacter(character);
                return;
            }
            else if (canSelectTeam && character.team == Character.Team.Player)
            {
                SelectCharacter(character);
                return;
            }
            else if (canSelectEnemies && character.team == Character.Team.Enemy)
            {
                SelectCharacter(character);
                return;
            }
        }

        UnSelectCharacter();
    }
    void CastCombatAction()
    {
        TurnManager.instance.GetCurrentTurnCharacter().CastCombatAction(curSelectionCombatAction, curSelectedCharacter);        
        curSelectionCombatAction = null;

        UnSelectCharacter();
        DisablePlayerCombat();
        combatActionsUI.DisableCombatActions();
        TurnManager.instance.endTurnButton.SetActive(false);

        Invoke(nameof(nextTurnDelay), 1f);
    }
    void nextTurnDelay()
    {
        TurnManager.instance.EndTurn();
    }
    void SelectCharacter(Character character)
    {
        UnSelectCharacter();
        curSelectedCharacter = character;
        character.ToggleSelectionVisual(true);
    }
    void UnSelectCharacter()
    {
        if (curSelectedCharacter == null)
            return;

        curSelectedCharacter.ToggleSelectionVisual(false);
        curSelectedCharacter = null;
    }
    public void SetCurrentCombatAction(CombatAction combatAction)
    {
        curSelectionCombatAction = combatAction;

        canSelectSelf = false;
        canSelectTeam = false;
        canSelectEnemies = false;

        if(combatAction as MeleeCombatAction || combatAction as RangedCombatAction)
        {
            canSelectEnemies = true;
        }
        else if (combatAction as HealCombatAction)
        {
            canSelectSelf = true;
            canSelectTeam = true;
        }
        else if (combatAction as EffectCombatAction)
        {
            canSelectSelf = (combatAction as EffectCombatAction).canEffectSelf;
            canSelectTeam = (combatAction as EffectCombatAction).canEffectTeam;
            canSelectEnemies = (combatAction as EffectCombatAction).canEffectEnemy;
        }
    }
}
