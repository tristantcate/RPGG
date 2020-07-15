using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattle : BattleParticipant
{
    private EnemyPrototype enemyPrototype;
    private SpriteRenderer m_spriteRenderer;

    public enum BattleState { Attack, Hurt };

    private void Awake() => m_spriteRenderer = GetComponent<SpriteRenderer>();

    public void SetupEnemy(EnemyPrototype a_enemyProtoType)
    {
        enemyPrototype = a_enemyProtoType;
        m_spriteRenderer.sprite = enemyPrototype.GetIdleSprite();

        m_battleStats = a_enemyProtoType.GetBattleStats();
        Setup();
    }

    public override IEnumerator WaitForBattleMove(List<BattleParticipant> a_possibleTargets)
    {
        temp_chosenMove = enemyPrototype.GetRandomBattleMove();

        List<BattleParticipant> consideredTargets = new List<BattleParticipant>();
        foreach (BattleParticipant possibleTarget in a_possibleTargets)
        {
            //Later on, we can program specific tactics for the enemies in here, like calling for help or targeting lower health players.
            //Also an option, set this in data and make this function adhere to the data, following a "tactics" data structure. <-

            //For now, simply attack a random player.
            if (possibleTarget is EnemyBattle) continue;
            consideredTargets.Add(possibleTarget);
        }

        int randomEnemySelector = Random.Range(0, consideredTargets.Count);
        temp_target = consideredTargets[randomEnemySelector];
        yield break;

    }

    public IEnumerator ProcessAnimation(BattleState a_battleState, Vector3 a_moveSprite, float a_waitTime)
    {
        m_statsBox.SetActive(false);
        if (a_battleState == BattleState.Attack) m_spriteRenderer.sprite = enemyPrototype.GetAttackSprite();
        if (a_battleState == BattleState.Hurt) m_spriteRenderer.sprite = enemyPrototype.GetHurtSprite();
        transform.position += a_moveSprite;

        yield return new WaitForSeconds(a_waitTime);

        transform.position -= a_moveSprite;
        m_spriteRenderer.sprite = enemyPrototype.GetIdleSprite();
        m_statsBox.SetActive(true);

    }


    public override string GetParticipantName() => enemyPrototype.name;
}
