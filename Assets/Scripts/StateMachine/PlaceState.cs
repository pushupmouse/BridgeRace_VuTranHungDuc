using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : IState
{
    private Transform groundTarget;
    public void OnEnter(Bot bot)
    {
        Debug.Log("Entered place state");
        groundTarget = bot.SetGroundTarget();
    }

    public void OnExecute(Bot bot)
    {
        if(bot.bricksCollected.Count > 0)
        {
            bot.GoTowardsTarget(groundTarget);
        }
        else
        {
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
