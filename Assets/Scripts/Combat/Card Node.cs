using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CardNode", menuName = "ScriptableObjects/CardNode", order = 2)]
public class CardNode : ScriptableObject
{
    public int cardHP;
    public int cardATK;
    public int cardSPD;

    public List<Skill> skills = new List<Skill>();
}

public enum EffectType
{
    Empty,
    Damage,
    Heal,
    TurnRotation,
}

public enum TargetType
{
    Empty,
    Self,
    Ally,
    Enemy,
}

[System.Serializable]
public class Skill
{
    public Sprite cardSprite;
    public List<SkillEffect> skillEffects = new List<SkillEffect>();
}

[System.Serializable]
public class SkillEffect
{
    [Header("Effect")]
    public EffectType effectType;
    public TargetType targetType;
    public int targetCount; //1, 2, all
    public int dealAmount; //ei. [Heal 10], [Damage 14]
    public int dealCount; //ei. Hit 5 damage [4] times
}

