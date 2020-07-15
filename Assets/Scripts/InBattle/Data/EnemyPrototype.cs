using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", order = 0)]
public class EnemyPrototype : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private BattleStats m_battleStats;
    [SerializeField] private BattleMove[] m_battleMoves;

    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite hurtSprite;

    public BattleStats GetBattleStats() => m_battleStats;
    public BattleMove GetRandomBattleMove()
    {
        int randomPercent = Random.Range(0, 100);
        int currentPercent = 0;
        foreach (BattleMove move in m_battleMoves)
        {
            currentPercent += move.movePercentage;
            if (randomPercent <= currentPercent) return move;
        }

        Debug.LogError("Didn't properly perform GetRandomBattleMove(), currentPercent = " + currentPercent);
        return m_battleMoves[0];
    }

    public Sprite GetIdleSprite() => idleSprite;
    public Sprite GetAttackSprite() => attackSprite;
    public Sprite GetHurtSprite() => hurtSprite;
    public string GetName() => m_name;

}
