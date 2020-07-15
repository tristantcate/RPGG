using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : BattleParticipant
{
    //Not pretty
    private void Awake()
    {
        Setup();
    }

    public override IEnumerator WaitForBattleMove(List<BattleParticipant> a_possibleTargets)
    {
        yield return null;
    }
}
