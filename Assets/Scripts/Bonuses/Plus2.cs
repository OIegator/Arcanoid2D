using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus2 : BonusBase
{
    PlayerScript playerObj;

    public override void BonusActivate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        gameData.balls += 2;
        playerObj.CreateBalls(2);
    }
}
