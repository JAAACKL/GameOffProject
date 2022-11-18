using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Skill;

public abstract class EnemyStatus: MonoBehaviour
{
    protected float MAX_HEALTH;
    protected float currentHealth;
    protected float happyATK;
    protected float happyDEF;
    protected float sadATK;
    protected float sadDEF;
    protected float angryATK;
    protected float angryDEF;
    protected int hitsTakenCounter;
    protected int attackCounter;
    protected List<Buff> buffs;
    protected List<Debuff> debuffs;   

    // process round counters for buffs and debuffs
    public void updateBuffDebuffStatus() {
        for (int i = buffs.Count - 1; i >= 0; i--) {
            if (buffs[i].decreaseCounter()) {
                buffs.RemoveAt(i);
            }
        }
        for (int i = debuffs.Count - 1; i >= 0; i--) {
            if (debuffs[i].decreaseCounter()) {
                debuffs.RemoveAt(i);
            }
        }
    }

    public bool activateBuff(Buff buff) {
        for (int i = 0; i < buffs.Count; i++) {
            if (buffs[i].GetBuffId() == buff.GetBuffId()) {
                buffs[i].resetDuration();
                return true;
            }
        }
        buffs.Insert(0, buff);
        return false;
    }

    public void clearBuff() {
        buffs.Clear();
    }

    private void Awake() {
        // for test purposes -
        MAX_HEALTH = 500.0f;
        currentHealth = MAX_HEALTH;
        happyATK = 50.0f;
        happyDEF = 50.0f;
        sadATK = 50.0f;
        sadDEF = 50.0f;
        angryATK = 50.0f;
        angryDEF = 50.0f;
        // - for test purposes
    }

     public float TakeDamage(float damage, SkillAttribute type) {
        // if immune, takes no damage, 
        // unless attribute is NONE, which means it is the effect of using immune
        if (buffs.Contains(new Buff(Buff.BuffId.IMMUNE)) && type != SkillAttribute.NONE) {
            return 0;
        }
        
        float effectiveDamage = damage * (50f / (50f + getDEFbyAttribute(type)));
        currentHealth -= effectiveDamage;
        if (currentHealth <= 0) {
            currentHealth = 0;
        }
        return effectiveDamage;
    }

    public float getATKbyAttribute(SkillAttribute attribute) {
        switch(attribute) {
            case SkillAttribute.HAPPY:
                return happyATK;
            case SkillAttribute.SAD:
                return sadATK;
            case SkillAttribute.ANGRY:
                return angryATK;
            default:
                return 0.0f;
        }
    }

    public float getDEFbyAttribute(SkillAttribute attribute) {
        switch(attribute) {
            case SkillAttribute.HAPPY:
                return happyDEF;
            case SkillAttribute.SAD:
                return sadDEF;
            case SkillAttribute.ANGRY:
                return angryDEF;
            default:
                return 0.0f;
        }
    }

    public abstract void makeMove(PlayerStatus playerStatus);
}
