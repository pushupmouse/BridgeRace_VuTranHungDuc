using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class CollectState : IState
{
    private Transform brickTarget;
    public void OnEnter(Bot bot)
    {
        Debug.Log("Entered collect state and set target");
        brickTarget = bot.SetBrickTarget();
    }
    public void OnExecute(Bot bot) 
    {
        if(brickTarget != null)
        {
            Debug.Log("Navigating to the brick");
            bot.GoTowardsTarget(brickTarget);
        }
        else
        {
            Debug.Log("Brick collected");
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot) 
    {
    
    }
}
