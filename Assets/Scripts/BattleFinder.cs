using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFinder : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;

    private BattleArea currentBattleArea;
    private float currentBattleThreshold = 0.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BattleArea thisBattleArea = collision.GetComponent<BattleArea>();
        if (thisBattleArea)
        {
            currentBattleArea = thisBattleArea;
            StartCoroutine(WaitForBattleThreshold(currentBattleArea.GetNewRandomSpawnTime()));
        }
    }

    public void UpdateStepCounter() => currentBattleThreshold += Time.deltaTime;

    IEnumerator WaitForBattleThreshold(float a_battleThreshold)
    {
        Debug.Log("Looking for Battle...");

        while (currentBattleThreshold < a_battleThreshold) yield return null;
        
        currentBattleThreshold = 0.0f;
        Debug.Log("Initiating Battle");


        //Initiate Battle
        battleSystem.SetupBattleScene(currentBattleArea.GenerateMonstersToSpawn(), transform.position);
        GetComponent<PlayerMovement>().FreezePlayer(true);
        yield break;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BattleArea thisBattleArea = collision.GetComponent<BattleArea>();
        if (thisBattleArea && thisBattleArea == currentBattleArea)
        {
            currentBattleArea = null;
            currentBattleThreshold = 0.0f;
            StopCoroutine("WaitForBattleThreshold");
        }
    }
}
