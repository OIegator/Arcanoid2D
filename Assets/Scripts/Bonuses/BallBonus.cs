using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBonus : BonusBase
{
    public override void BonusActivate()
    {
        gameData.balls += 1;
    }
}
