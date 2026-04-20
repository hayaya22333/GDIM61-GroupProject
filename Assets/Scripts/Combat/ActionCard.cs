using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionCard : DragObject
{
    [SerializeField] private CombatController combatController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameSide targetSide;
    [SerializeField] private GameSide targetSide2;
    [SerializeField] private PlayerCard parentCard;
    public TextMeshPro displayText;

    [Header("Skill Attributes")]
    public GameSide side;
    public int id;
    bool hasCasted = false;
    int castLeft = -1;

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

    void Awake()
    {
        displayText.text = "";
    }
    
    void Start()
    {
        combatController = CombatLocator.Instance.Controller;
    }

    void Update()
    {
        if (castLeft == 0)
        {
            Debug.Log("End turn from card " + id);
            parentCard.EndTurn();
        }
    }

    public void SetAttributes(Skill _skill, PlayerCard _parentCard, int _id)
    {
        id = _id;
        parentCard = _parentCard;
        side = _parentCard.side;
        spriteRenderer.sprite = _skill.cardSprite;
        
        if (_skill.effectType == EffectType.Empty)
        {
            Debug.Log("Empty skill found. Please check the Skills dropdown under the scriptable object attached to card " + id + "and make sure Effect 1 is filled.");
            return;
        }
        effectType = _skill.effectType;
        targetType = _skill.targetType;
        targetCount = _skill.targetCount;
        dealAmount = _skill.dealAmount;
        dealCount = _skill.dealCount;
        castLeft = targetCount;
        targetSide = GetTargetSide(effectType,
            targetType,
            targetCount,
            dealAmount,
            dealCount
        );

        if (_skill.effectType2 == EffectType.Empty)
        {
            return;
        }
        displayText.text += " [AND] ";
        effectType2 = _skill.effectType2;
        targetType2 = _skill.targetType2;
        targetCount2 = _skill.targetCount2;
        dealAmount2 = _skill.dealAmount2;
        dealCount2 = _skill.dealCount2;
        castLeft += targetCount2;
        targetSide2 = GetTargetSide(effectType2,
            targetType2,
            targetCount2,
            dealAmount2,
            dealCount2
        );
    }

    public GameSide GetTargetSide(EffectType _effectType,
        TargetType _targetType,
        int _targetCount,
        int _dealAmount,
        int _dealCount
    )
    {
        if (_effectType == EffectType.Heal)
        {
            displayText.text += "Heal ";
        }
        else
        {
            displayText.text += "Damage ";
        }
        
        GameSide _targetSide = GameSide.Neutral;
        switch(_targetType)
        {
            case TargetType.Enemy:
                _targetSide = GameSide.Enemy;
                displayText.text += "enemy ";
                break;
            case TargetType.Ally:
                _targetSide = GameSide.Player;
                displayText.text += "ally ";
                break;
            case TargetType.Self:
                _targetSide = GameSide.Player;
                displayText.text += "self ";
                break;
            default:
                Debug.Log("Invalid side on skill card " + id + " part 2. Check scriptable object's targetType field");
                break;
        }

        displayText.text += "for " + Mathf.Abs(_dealAmount).ToString() + " points";

        if (_dealCount > 1)
        {
            displayText.text += " " + _dealCount + " times";
        }

        return _targetSide;
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
                if (effectType == EffectType.Heal)
                {
                    dealAmount *= -1;
                }
                // First, cast on designated target
                StartCoroutine(CastOnTarget(target.id));
                // If multiple target, cast on random other targets
                if (targetCount > 1)
                {
                    CastMultiple(targetCount - 1, targetType, target.id);
                }
                // Temporary solution for cast feedback
                spriteRenderer.enabled = false;
                displayText.enabled = false;
                hasCasted = true;
            }
        }
    }

    private void CastMultiple(int _targetCount, TargetType _targetType, int _excludeID)
    {
        List<int> availableTargets = new List<int>();
        int randTargetID = -1;
        if (_targetType == TargetType.Ally)
        {
            availableTargets = new List<int>(combatController.playerIDs);
        }
        else if (_targetType == TargetType.Enemy)
        {
            availableTargets = new List<int>(combatController.enemyIDs);
        }
        else if (_targetType == TargetType.Self)
        {
            // TODO implement target -> self
            castLeft -= _targetCount;
            return;
        }
        availableTargets?.Remove(_excludeID);

        for (int i = 1; i <= _targetCount; i++)
        {
            if (availableTargets.Count == 0)
            {
                castLeft -= (_targetCount - i + 1);
                return;
            }
            randTargetID = availableTargets[UnityEngine.Random.Range(0, availableTargets.Count)];
            availableTargets?.Remove(randTargetID);
            StartCoroutine(CastOnTarget(randTargetID));
        }
    }

    IEnumerator CastOnTarget(int _targetID)
    {
        for (int i = 1; i <= dealCount; i++)
        {
            combatController.InflictAttack(parentCard.id, _targetID, dealAmount);
            if (i != dealCount) yield return new WaitForSeconds(0.4f);
        }
        castLeft -= 1;
    }
}
