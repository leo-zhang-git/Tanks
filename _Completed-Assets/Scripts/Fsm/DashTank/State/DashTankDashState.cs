using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class DashTankDashState : FsmState<DashTankController>
    {
        protected internal override void OnEnter(DashTankController fsm)
        {
            base.OnEnter(fsm);
        }
        protected internal override void OnUpdate(DashTankController fsm, float elapseSeconds, float realElapseSeconds)
        {
            if(fsm.target== null) ChangeState<DashTankPatrolState>(fsm);
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            fsm.agent.SetDestination(fsm.target.position);
        }
    }
}
