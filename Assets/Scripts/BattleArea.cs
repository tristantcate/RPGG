using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BattleArea : MonoBehaviour
{
    [Header("Spawn Time")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    [Header("Monster Encounters")]
    [SerializeField] private int minMonsterEncounters;
    [SerializeField] private int maxMonsterEncounters;

    [Space(8)]
    [SerializeField] private MonsterEncounter[] monsterEncounters;

    [System.Serializable]
    public struct MonsterEncounter
    {
        public EnemyPrototype enemyPrototype;
        public int spawnChance;
    }

    public float GetNewRandomSpawnTime() => Random.Range(minSpawnTime, maxSpawnTime);
    public List<EnemyPrototype> GenerateMonstersToSpawn()
    {
        List<EnemyPrototype> monstersToSpawn = new List<EnemyPrototype>();

        int randomMonsterAmount = Random.Range(minMonsterEncounters, maxMonsterEncounters+1);
        for (int i = 0; i < randomMonsterAmount; i++)
        {
            //Randomly get one of the monsters in the monsterEncounters array.
            int randomPercent = Random.Range(0, 100);
            int percentCount = 0;

            foreach (MonsterEncounter monster in monsterEncounters)
            {
                percentCount += monster.spawnChance;
                if (randomPercent <= percentCount)
                {
                    monstersToSpawn.Add(monster.enemyPrototype);
                    break;
                }
            }
        }

        if (monstersToSpawn.Count == 0) Debug.LogError("No Monster Found in MonstersToSpawn List!");
        return monstersToSpawn;
    }
}
