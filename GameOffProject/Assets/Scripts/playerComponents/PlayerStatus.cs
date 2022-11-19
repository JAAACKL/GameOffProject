using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using static Eye;
using static EyeBrow;
using static Mouth;
using static Skill;
using static Buff;
using static EnemyStatus;

public class PlayerStatus : MonoBehaviour
{
    public const float MAX_HEALTH = 120;
    private float currentHealth;
    public float getCurrentHealth() {
        return currentHealth;
    } 
    private float happyATK;
    private float happyDEF;
    private float sadATK;
    private float sadDEF;
    private float angryATK;
    private float angryDEF;

    private bool immuneNextATK;
    private bool reflectNextATK;
    

    // a list of currently active BUFFs
    // ? a list of currently active DeBUFFs? Maybe put this on enemy status

    // Current Skills: List of size 3?4
    // idx 0: attack skill
    // idx 1: defense skill
    // idx 2: buff/debuff skill
   
    private List<Buff> buffs;
    public List<Buff> GetActiveBuffs() {
        return buffs;
    } 

    // process round counters for buffs and debuffs
    public void UpdateEffectStatus() {
        for (int i = buffs.Count - 1; i >= 0; i--) {
            if (buffs[i].decreaseCounter()) {
                switch (buffs[i].GetBuffId())
                {
                    case BuffId.FORTIFIED:
                        //restore def of player
                        break;
                    case BuffId.REDUCED:
                        //restore atk of player
                        break;
                    default:
                }
                buffs.RemoveAt(i);
            }
        }
    }

    public bool ActivateBuff(Buff buff) {
        //play buff animation here ??? 
        for (int i = 0; i < buffs.Count; i++) {
            if (buffs[i].GetBuffId() == buff.GetBuffId()) {
                buffs[i].resetDuration();
                return true;
            }
        }
        buffs.Insert(0, buff);
        return false;
    }

    public void ClearBuff() {
        buffs.Clear();
    }

    private List<Skill> skills;
    public List<Skill> GetSkills() {return skills;}

    private EyeBrow equippedEyebrow;
    private Eye equippedEyes;
    private Mouth equippedMouth;
    private List<EyeBrow> ownedEyebrows = new List<EyeBrow>();
    private List<Eye> ownedEyes = new List<Eye>();
    private List<Mouth> ownedMouth = new List<Mouth>();

    private List<Button> skillSet = new List<Button>();

    private void Awake() {
        // for test purposes -
        equippedEyebrow = new EyeBrow(SkillAttribute.NONE);
        equippedEyes = new Eye(SkillAttribute.NONE);
        equippedMouth = new Mouth(SkillAttribute.NONE);

        ownedEyebrows.Add(equippedEyebrow);
        ownedEyebrows.Add(new EyeBrow(SkillAttribute.HAPPY));
        ownedEyebrows.Add(new EyeBrow(SkillAttribute.SAD));
        ownedEyes.Add(equippedEyes);
        ownedMouth.Add(equippedMouth);
        updateStatus();

        ownedClues.Add(new Clue(0));
        ownedClues.Add(new Clue(5));
        ownedClues.Add(new Clue(6));
        ownedClues.Add(new Clue(12));
        // - for test purposes
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MAX_HEALTH;
        // initialize eyes, eyebrow, mouth
        // initialize skills based on equipments.
    }
    
    public float TakeDamage(float damage, SkillAttribute type) {
        // if immune, takes no damage, 
        // unless attribute is NONE, which means it is the effect of using immune
        if (buffs.Contains(new Buff(Buff.BuffId.IMMUNE)) && type != SkillAttribute.NONE) {
            return 0;
        }

        // if reflect, takes no damage, and deal damage to opponent 
        // NOT of same value since enemy DEF is different from player
        // unless attribute is NONE, which means it is the effect of using reflect
        if (buffs.Contains(new Buff(Buff.BuffId.REFLECT)) && type != SkillAttribute.NONE) {
            EnemyStatus.TakeDamage(damage, type);
            return 0;
        }
        
        float effectiveDamage = damage * (50f / (50f + getDEFbyAttribute(type)));
        currentHealth -= effectiveDamage;
        if (currentHealth <= 0) {
            currentHealth = 0;
        }
        return effectiveDamage;
    }

    public void ProcessHealing(float healAmount) {
        currentHealth += healAmount;
        if (currentHealth > MAX_HEALTH) {
            currentHealth = MAX_HEALTH;
        }
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

    public void setHappyATK(float happyATK) {
        this.happyATK = happyATK;
    }

    public void setHappyDEF(float happyDEF) {
        this.happyDEF = happyDEF;
    }

    public void setSadATK(float sadATK) {
        this.sadATK = sadATK;
    }

    public void setSadDEF(float sadDEF) {
        this.sadDEF = sadDEF;
    }

    public void setAngryATK(float angryATK) {
        this.angryATK = angryATK;
    }

    public void setAngryDEF(float angryDEF) {
        this.angryDEF = angryDEF;
    }

    public void setSkills(List<Skill> skills) {
        this.skills = skills;
    }

    public EyeBrow getEquippedEyeBrow() {
        return equippedEyebrow;
    }

    public Eye getEquippedEyes() {
        return equippedEyes;
    }

    public Mouth getEquippedMouth() {
        return equippedMouth;
    }

    public void setEquippedMouth(Mouth m) {
        equippedMouth = m;
    }

    public void setEquippedEyeBrow(EyeBrow eb) {
        equippedEyebrow = eb;
    }

    public void setEquippedEyes(Eye e) {
        equippedEyes = e;
    }

    public void addEyebrow(SkillAttribute attribute) {
        ownedEyebrows.Add(new EyeBrow(attribute));
    }

    public void addEye(SkillAttribute attribute) {
        ownedEyes.Add(new Eye(attribute));
    }

    public void addMouth(SkillAttribute attribute) {
        ownedMouth.Add(new Mouth(attribute));
    }

    public List<EyeBrow> getOwnedEyeBrows() {
        return ownedEyebrows;
    }

    public List<Eye> getOwnedEyes() {
        return ownedEyes;
    }

    public List<Mouth> getOwnedMouths() {
        return ownedMouth;
    }

    public void updateStatus() {

        //(TODO) UPDATE NEEDS TO CHECK BUFFS 

        setHappyATK(equippedEyebrow.getHappyATK() + equippedEyes.getHappyATK() + equippedMouth.getHappyATK());
        setHappyDEF(equippedEyebrow.getHappyDEF() + equippedEyes.getHappyDEF() + equippedMouth.getHappyDEF());
        setSadATK(equippedEyebrow.getSadATK() + equippedEyes.getSadATK() + equippedMouth.getSadATK());
        setSadDEF(equippedEyes.getSadDEF() + equippedEyes.getSadDEF() + equippedMouth.getSadDEF());
        setAngryATK(equippedEyebrow.getAngryATK() + equippedEyes.getAngryATK() + equippedMouth.getAngryATK());
        setAngryDEF(equippedEyebrow.getAngryDEF() + equippedEyes.getAngryDEF() + equippedMouth.getAngryDEF());

        List<Skill> newSkills = new List<Skill>();
        // idx 0: attack skill
        newSkills.Add(equippedEyes.getSkill());
        // idx 1: defense skill
        newSkills.Add(equippedEyebrow.getSkill());
        // idx 2: buff/debuff skill
        newSkills.Add(equippedMouth.getSkill());
        setSkills(newSkills);
    }

    private List<Clue> ownedClues = new List<Clue>();
    public void addClue(int id) {
        ownedClues.Add(new Clue(id));
    }
    public Clue getClue(int id)
    {
        for (int i = 0; i < ownedClues.Count; i++) {
            if (ownedClues[i].getClueId() == id) {
                return ownedClues[i];
            }
        }
        return new Clue(-1);
    }
}
