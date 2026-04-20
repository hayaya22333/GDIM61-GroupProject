using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionCard : DragObject
{
    [SerializeField] private CombatController combatController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameSide targetSide;
    [SerializeField] private PlayerCard parentCard;
    public TextMeshPro displayText;

    [Header("Skill Attributes")]
    public GameSide side;
    public int id;
    bool hasCasted = false;

    [Header("Effect 1")]
    public EffectType effectType;
    public TargetType targetType;
    public int targetCount;
    public int dealAmount;
    public int dealCount;

    [Header("Effect 2 (optional)")]
    public EffectType effectType2;
    public TargetType targetType2;
    public int targetCount2;
    public int dealAmount2;
    public int dealCount2;

    void Start()
    {
        combatController = CombatLocator.Instance.Controller;
    }

    public void SetAttributes(Skill _skill, PlayerCard _parentCard, int _id)
    {
        id = _id;
        parentCard = _parentCard;
        side = _parentCard.side;
        
        if (_skill.effectType == EffectType.Empty)
        {
            Debug.Log("Empty skill found. Please check the Skills dropdown under the scriptable object attached to card " + id + "and make sure Effect 1 is filled.");
            return;
        }

        spriteRenderer.sprite = _skill.cardSprite;

        effectType = _skill.effectType;
        targetType = _skill.targetType;
        targetCount = _skill.targetCount;
        dealAmount = _skill.dealAmount;
        dealCount = _skill.dealCount;

        effectType2 = _skill.effectType;
        targetType2 = _skill.targetType2;
        targetCount2 = _skill.targetCount2;
        dealAmount2 = _skill.dealAmount2;
        dealCount2 = _skill.dealCount2;

        if (effectType == EffectType.Heal)
        {
            dealAmount *= -1;
            displayText.text = "Heal ";
        }
        else
        {
            displayText.text = "Damage ";
        }

        switch(targetType)
        {
            case TargetType.Enemy:
                targetSide = GameSide.Enemy;
                displayText.text += "enemy ";
                break;
            case TargetType.Ally:
                targetSide = GameSide.Player;
                displayText.text += "ally ";
                break;
            case TargetType.Self:
                targetSide = GameSide.Player;
                displayText.text += "self ";
                break;
            default:
                Debug.Log("Invalid side on skill card " + id + ". Check scriptable object's targetType field");
                break;
        }

        displayText.text += "for " + Mathf.Abs(dealAmount).ToString() + " points.";

        if (dealCount > 1)
        {
            displayText.text += " for " + dealCount + " times";
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDragging) return;

        if (other.TryGetComponent<GeneralCombatCard>(out GeneralCombatCard target))
        {
            // Check if dragged to expected target
            if (target.side != targetSide) return;

            if (targetType == TargetType.Self)
            {
                if (target.id != parentCard.id) return;
            }

            if (!hasCasted)
            {
                StartCoroutine(Cast(target.id));
                spriteRenderer.enabled = false;
                displayText.enabled = false;
                hasCasted = true;
            }
        }
    }

    IEnumerator Cast(int targetID)
    {
        for (int i = 1; i <= dealCount; i++)
        {
            combatController.InflictAttack(id, targetID, dealAmount);
            if (i != dealCount) yield return new WaitForSeconds(0.5f);
        }
        parentCard.EndTurn();
        Destroy(gameObject);
    }
}
