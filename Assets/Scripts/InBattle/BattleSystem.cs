using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform enemyParent;

    [SerializeField] private GameObject enemySlotPrefab;
    private List<GameObject> enemySlots = new List<GameObject>();

    [SerializeField] private float slotDistance;

    bool battleFinished = false;

    private BattleUI m_battleUI;

    private void Awake()
    {
        m_battleUI = GetComponent<BattleUI>();
    }

    public void SetupBattleScene(List<EnemyPrototype> a_enemyPrototypes, Vector3 a_playerPosition)
    {
        ActivateBattleScene(a_playerPosition);
        foreach (EnemyPrototype enemyProt in a_enemyPrototypes)
        {
            AddEnemy(enemyProt);
        }

        PositionEnemySlots();
        StartCoroutine(BattlePhasing());
    }

    public void ActivateBattleScene(Vector3 a_playerPosition)
    {
        //Intro to battle animation?
        Transform battleScene = transform.GetChild(0);
        battleScene.gameObject.SetActive(true);
        battleScene.position = a_playerPosition;
        
    }

    public void AddEnemy(EnemyPrototype a_enemy)
    {
        GameObject newEnemySlot = Instantiate(enemySlotPrefab, enemyParent);
        newEnemySlot.GetComponent<EnemyBattle>().SetupEnemy(a_enemy);
        enemySlots.Add(newEnemySlot);
    }

    private IEnumerator BattlePhasing()
    {

        //Get all Battle Participants, order them by speed
        List<BattleParticipant> participants = GetAllBattleParticipants();

        //Every round (all entities have had 1 turn)
        while (!battleFinished)
        {
            bool fullRound = true;

            //Loop through each battle participant, and ask what they want to do
            foreach (BattleParticipant participant in participants)
            {
                if (participant.m_hadTurn) continue;

                BattleMove chosenMove;
                BattleParticipant chosenTarget;

                if(participant is EnemyBattle)
                {
                    yield return StartCoroutine(participant.WaitForBattleMove(participants));
                    participant.GetBattleAction(out chosenMove, out chosenTarget);
                }
                else
                {
                    yield return StartCoroutine(m_battleUI.AskAndWaitForPlayer(participant, participants));
                    m_battleUI.GetBattleAction(out chosenMove, out chosenTarget);
                }
                

                //Log action to outputBox
                string logString = string.Format(
                    "\n {0} uses {1} against {2}!",
                    participant.GetParticipantName(),
                    chosenMove.moveName,
                    chosenTarget.GetParticipantName()
                    );

                yield return StartCoroutine(m_battleUI.LogToConsoleBox(logString));


                //Animation
                EnemyBattle enemyChosen = chosenTarget as EnemyBattle;
                if (enemyChosen) StartCoroutine(enemyChosen.ProcessAnimation(EnemyBattle.BattleState.Hurt, Vector3.up * 0.3f, 0.3f));
                EnemyBattle enemyFighting = participant as EnemyBattle;
                if (enemyFighting) StartCoroutine(enemyFighting.ProcessAnimation(EnemyBattle.BattleState.Attack, Vector3.down * 0.5f, 0.5f));

                //Process damage, more elaborate later on.
                int processedDamage;
                chosenTarget.TakeDamage(chosenMove.damageModifier * participant.GetBattleStats().attackPower, chosenMove.isMagicAttack, out processedDamage);



                //Log action result
                yield return StartCoroutine(m_battleUI.LogToConsoleBox(string.Format(("\n {0} takes {1} damage!"), chosenTarget.GetParticipantName(), processedDamage.ToString())));
                yield return new WaitForSeconds(0.5f);

                participant.m_hadTurn = true;

                List<BattleParticipant> deadParticipants = CheckIfParticipantsDied(participants);
                if(deadParticipants.Count > 0)
                {
                    foreach (BattleParticipant deadParticipant in deadParticipants)
                    {
                        yield return StartCoroutine(m_battleUI.LogToConsoleBox(string.Format(("\n {0} defeated!"), deadParticipant.GetParticipantName())));
                        Destroy(deadParticipant.gameObject);
                        yield return null; //Needs to be called as Destroy runs the same frame, but is delayed.
                    }

                    participants.Clear();
                    participants = GetAllBattleParticipants();

                    if (enemyParent.childCount == 0)
                    {
                        yield return StartCoroutine(m_battleUI.LogToConsoleBox(string.Format("\n YOU WIN!")));
                        battleFinished = true;
                    }
                    else if (playerParent.childCount == 0)
                    {
                        yield return StartCoroutine(m_battleUI.LogToConsoleBox(string.Format("\n YOU LOSE!")));
                        battleFinished = true;
                    }

                    fullRound = false;
                    break;
                }
            }

            if (fullRound)
                foreach (BattleParticipant participant in participants) participant.m_hadTurn = false;

            if (battleFinished)
            {

                yield break;
            }
        }
    }

    private List<BattleParticipant> GetAllBattleParticipants()
    {
        List<BattleParticipant> returnParticipants = new List<BattleParticipant>();

        foreach (BattleParticipant participant in playerParent.GetComponentsInChildren<BattleParticipant>()) returnParticipants.Add(participant);
        foreach (BattleParticipant participant in enemyParent.GetComponentsInChildren<BattleParticipant>()) returnParticipants.Add(participant);
        returnParticipants.OrderBy(s => s.GetBattleStats().speed);

        return returnParticipants;
    }


    private List<BattleParticipant> CheckIfParticipantsDied(List<BattleParticipant> a_checkDeadParticipants)
    {
        List<BattleParticipant> deadParticipants = new List<BattleParticipant>();

        foreach (BattleParticipant checkDeadParticipant in a_checkDeadParticipants)
        {
            if (checkDeadParticipant.currentHealth <= 0)
            {
                deadParticipants.Add(checkDeadParticipant);              
            }
        }
        return deadParticipants;
    }

    private void PositionEnemySlots()
    {
        float xAnchor = (-slotDistance * enemySlots.Count) / 2.0f + slotDistance / 2.0f;

        for(int i = 0; i < enemySlots.Count; i++)
        {
            float xPos = xAnchor + i * slotDistance;
            enemySlots[i].transform.localPosition = Vector3.right * xPos; 
        }
    }
}
