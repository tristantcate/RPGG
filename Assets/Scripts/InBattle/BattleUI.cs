using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private GameObject consoleBox;
    private Text consoleBoxText;
    [SerializeField] private GameObject actionSelectionMenu;
    [SerializeField] private GameObject targetSelectionMenu;

    //public enum PlayerAction { NotChosen, Attack, Inspect, Magic /*Different kinds later*/ , Run }
    [SerializeField] private BattleMove attackMove;
    [HideInInspector] public BattleMove chosenPlayerMove;
    [HideInInspector] public BattleParticipant chosenTarget;

    private string m_entireBattleLog;
    private WaitForSeconds waitForText = new WaitForSeconds(0.5f);


    private void Awake()
    {
        consoleBoxText = consoleBox.GetComponentInChildren<Text>();
    }

    public IEnumerator AskAndWaitForPlayer(BattleParticipant a_thisPlayer, List<BattleParticipant> a_possibleTargets)
    {
        foreach (BattleParticipant battler in a_possibleTargets) {
            battler.GetComponentInChildren<Button>().onClick.AddListener(() => SelectTarget(battler));
        }
        Debug.Log(a_possibleTargets.Count);

        OpenActionSelectionMenu();

        while (chosenTarget == null) yield return null;
        consoleBox.SetActive(true);
    }

    public void GetBattleAction(out BattleMove a_chosenMove, out BattleParticipant a_chosenTarget)
    {
        a_chosenMove = chosenPlayerMove;
        a_chosenTarget = chosenTarget;
        chosenTarget = null;
    }

    public void SelectTarget(BattleParticipant a_thisTarget) => chosenTarget = a_thisTarget;

    public void AttackButton()
    {
        chosenPlayerMove = attackMove;
        actionSelectionMenu.SetActive(false);
        targetSelectionMenu.SetActive(true);
        OpenTargetSelectionMenu();
    }

    public void BackButton()
    {
        OpenActionSelectionMenu();
    }

    private void OpenActionSelectionMenu()
    {
        consoleBox.SetActive(false);
        targetSelectionMenu.SetActive(false);

        //Animation Later, make function that calls enumerator and disables until animation done.
        actionSelectionMenu.SetActive(true);

    }

    private void OpenTargetSelectionMenu()
    {
        consoleBox.SetActive(false);
        actionSelectionMenu.SetActive(false);
        targetSelectionMenu.SetActive(true);
    }

    public IEnumerator LogToConsoleBox(string a_logText)
    {
        m_entireBattleLog += a_logText;
        consoleBoxText.text = a_logText;
        yield return waitForText;
    }
}
