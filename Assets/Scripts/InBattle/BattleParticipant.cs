using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleParticipant : MonoBehaviour
{
    [SerializeField] protected BattleStats m_battleStats;

    //Change into currentBattleStats, as all stats can potentially be temporarily affected!
    [HideInInspector] public int currentHealth;
    [HideInInspector] public int currentMana;

    [SerializeField] protected GameObject m_statsBox;
    [SerializeField] private Text m_nameText;
    [Space(8)]
    [SerializeField] private Text m_currentHealthText;
    [SerializeField] private Text m_maxHealthText;
    [SerializeField] private Text m_currentManaText;
    [SerializeField] private Text m_maxManaText;
    [Space(8)]
    [SerializeField] private Image m_healthBar;
    [SerializeField] private Image m_manaBar;

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
        m_currentHealthText.text = string.Format("{0}", currentHealth);
        m_maxHealthText.text = string.Format("{0}", m_battleStats.maxHealth);
        m_healthBar.fillAmount = currentHealth / (float) m_battleStats.maxHealth;
    }

    protected void UpdateManaUI()
    {
        //Advanced healthbars etc later.
        m_currentManaText.text = string.Format("{0}", currentMana);
        m_maxManaText.text = string.Format("{0}", m_battleStats.maxMana);
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