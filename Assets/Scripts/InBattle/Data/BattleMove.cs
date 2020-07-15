using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BattleMove
{
    public string moveName;
    public int damageModifier; // Is not the damage itself, rather just multiplies the attack or magic damage by this number.
    public int manaUsed;
    public bool isMagicAttack;

    [Space(7)]
    public int movePercentage;

    //Potentially put status effects and other variables in here.
}
