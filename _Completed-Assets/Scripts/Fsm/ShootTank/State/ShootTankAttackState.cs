using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class ShootTankAttackState : FsmState<ShootTankController>
    {
        const float attackRange = 25f;
        const float attackCD = 1.5f;
        float curCD;
        protected internal override void OnEnter(ShootTankController fsm)
        {
            base.OnEnter(fsm);
            curCD = 0;
        }
        protected internal override void OnUpdate(ShootTankController fsm, float elapseSeconds, float realElapseSeconds)
        {
            if (fsm.IsPause) return;
            if (fsm.target == null) ChangeState<ShootTankPatrolState>(fsm);
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            curCD -= Time.deltaTime;
            if(Vector3.SqrMagnitude(fsm.target.position - fsm.transform.position) <= attackRange * attackRange &&
                Vector3.Angle(fsm.transform.forward, fsm.target.position - fsm.transform.position) < 10f)
            {
                fsm.agent.isStopped= true;
                if(curCD <= 0)
                {
                    fsm.tankShooting.AutoFire(fsm.target.position);
                    curCD = attackCD;
                }
            }
            else
            {
                fsm.agent.isStopped= false;
                fsm.agent.SetDestination(fsm.target.position);
            }
        }
    }
}
