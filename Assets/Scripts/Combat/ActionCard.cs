using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionCard : DragObject
{
    [SerializeField] private CombatController combatController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerCard parentCard;
    public TextMeshPro displayText;

    [Header("Skill Attributes")]
    public GameSide side;
    public int id;

    bool hasCasted = false;
    int castLeft = -1;

    List<EffectType> effectTypes = new List<EffectType>();
    List<TargetType> targetTypes = new List<TargetType>();
    List<int> targetCounts = new List<int>();
    List<int> dealAmounts = new List<int>();
    List<int> dealCounts = new List<int>();
    List<GameSide> targetSides = new List<GameSide>();
    
    void Start()
    {
        combatController = CombatLocator.Instance.Controller;
    }

    void Update()
    {
        Debug.Log(castLeft);
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
        displayText.text = "";

        foreach (SkillEffect effect in _skill.skillEffects)
        {
            if (effect.effectType == EffectType.Empty)
            {
                return;
            }
            
            effectTypes.Add(effect.effectType);
            targetTypes.Add(effect.targetType);
            targetCounts.Add(effect.targetCount);
            dealAmounts.Add(effect.dealAmount);
            dealCounts.Add(effect.dealCount);

            if (effectTypes.Count > 1)
            {
                castLeft += effect.targetCount;
                displayText.text += " <AND> ";
            }
            else
            {
                castLeft = effect.targetCount;
            }
            targetSides.Add(GetTargetSide(effect.effectType,
                effect.targetType,
                effect.targetCount,
                effect.dealAmount,
                effect.dealCount
                ));
        }
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
            if (target.side != targetSides[0]) return;

            if (targetTypes[0] == TargetType.Self)
            {
                if (target.id != parentCard.id) return;
            }

            if (!hasCasted)
            {
                StartCast(target.id);
            }
        }
    }

    private void StartCast(int _targetID)
    {
        for (int i = 0; i < effectTypes.Count; i++)
        {
            EffectType _effectType = effectTypes[i];
            TargetType _targetType = targetTypes[i];
            int _targetCount = targetCounts[i];
            int _dealAmount = dealAmounts[i];
            int _dealCount = dealCounts[i];
            GameSide targetSide = targetSides[i];

            if (_effectType == EffectType.Heal)
            {
                _dealAmount *= -1;
            }

            if (i == 0) // Scenario for first effect
            {
                StartCoroutine(CastOnTarget(_targetID, _dealCount, _dealAmount));
                if (_targetCount > 1)
                {
                    CastMultiple(_targetCount - 1, _targetType, _targetID, _dealCount, _dealAmount);
                }
            }
            else
            {
                CastMultiple(_targetCount, _targetType, -1, _dealCount, _dealAmount);
            }
            spriteRenderer.enabled = false;
            displayText.enabled = false;
            hasCasted = true;
        }
    }

    private void CastMultiple(int _targetCount, TargetType _targetType, int _excludeID, int _dealCount, int _dealAmount)
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
            StartCoroutine(CastOnTarget(randTargetID, _dealCount, _dealAmount));
        }
    }

    IEnumerator CastOnTarget(int _IEtargetID, int _IEdealCount, int _IEdealAmount)
    {
        for (int i = 1; i <= _IEdealCount; i++)
        {
            combatController.InflictAttack(parentCard.id, _IEtargetID, _IEdealAmount);
            if (i != _IEdealCount) yield return new WaitForSeconds(0.4f);
        }
        castLeft -= 1;
    }
}
