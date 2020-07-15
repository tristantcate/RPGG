using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleParticipant : MonoBehaviour
{
    [SerializeField] protected BattleStats m_battleStats;

    [HideInInspector] public int currentHealth;
    [HideInInspector] public int currentMana;

    [SerializeField] protected GameObject m_statsBox;
    [SerializeField] private Text m_nameText;
    [SerializeField] private Text m_healthText;

    protected BattleMove temp_chosenMove;
    protected BattleParticipant temp_target;

    private string placeHolderName = "Player(Placeholder)";

    [HideInInspector] public bool m_hadTurn;

    protected void Setup()
    {
        currentHealth = m_battleStats.maxHealth;
        currentMana = m_battleStats.maxMana;
        m_nameText.text = GetParticipantName();
        UpdateHealthUI();
    }

    protected void UpdateHealthUI()
    {
        //Advanced healthbars etc later.
        m_healthText.text = string.Format("Health: {0}/{1}", currentHealth, m_battleStats.maxHealth);
    }

    public abstract IEnumerator WaitForBattleMove(List<BattleParticipant> a_possibleTargets);

    public virtual void GetBattleAction(out BattleMove a_chosenMove, out BattleParticipant a_target)
    {
        a_chosenMove = temp_chosenMove;
        a_target = temp_target;
    }

    public void TakeDamage(int a_damage, bool a_magicAttack, out int a_processedDamage)
    {
        int defensiveStat = m_battleStats.defense;
        if (a_magicAttack) defensiveStat = m_battleStats.magicDefense;

        //Calculate damage done according to defenses,
        //for now we only use the attack and health properties

        currentHealth -= a_damage;
        a_processedDamage = a_damage;
        UpdateHealthUI();
    }

    public BattleStats GetBattleStats() => m_battleStats;
    public virtual string GetParticipantName() => placeHolderName;
   
}