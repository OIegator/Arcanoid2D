using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus10 : BonusBase
{

    PlayerScript playerObj;

    public override void BonusActivate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        gameData.balls += 10;
        playerObj.CreateBalls(10);
    }
}

