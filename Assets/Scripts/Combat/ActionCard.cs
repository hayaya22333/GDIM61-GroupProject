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

    [Header("Skill Attributes")]
    public GameSide side;
    public int id;

    [Header("Effect 1")]
    public EffectType effectType;
    public TargetType targetType;
    public int targetCount;
    public int dealAmount;

    [Header("Effect 2 (optional)")]
    public EffectType effectType2;
    public TargetType targetType2;
    public int targetCount2;
    public int dealAmount2;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

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

        effectType2 = _skill.effectType;
        targetType2 = _skill.targetType2;
        targetCount2 = _skill.targetCount2;
        dealAmount2 = _skill.dealAmount2;

        switch(targetType)
        {
            case TargetType.Enemy:
                targetSide = GameSide.Enemy;
                break;
            case TargetType.Ally:
                targetSide = GameSide.Player;
                break;
            case TargetType.Self:
                targetSide = GameSide.Player;
                break;
            default:
                Debug.Log("Invalid side on skill card " + id + ". Check scriptable object's targetType field");
                break;
        }

        if (effectType == EffectType.Heal)
        {
            dealAmount *= -1;
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

            combatController.InflictAttack(id, target.id, dealAmount);
            
            parentCard.EndTurn();
            Destroy(gameObject);
        }
    }
}
