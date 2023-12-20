using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("Entered idle state");
    }

    public void OnExecute(Bot bot)
    {
        if(bot.bricksCollected.Count < 5)
        {
            Debug.Log("Not enough bricks");

            bot.ChangeState(new CollectState());
        }
        else
        {
            bot.ChangeState(new PlaceState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
