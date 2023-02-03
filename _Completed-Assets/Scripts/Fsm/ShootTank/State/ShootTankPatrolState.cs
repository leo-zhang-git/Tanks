using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Complete
{
    public class ShootTankPatrolState : FsmState<ShootTankController>
    {
        const float searchRange = 30f;
        const float searchAngle = 120f;
        const float refreshTime = 3f;
        const float patrolRadius = 5f;

        private float timeCnt = 0f;
        protected internal override void OnEnter(ShootTankController fsm)
        {
            base.OnEnter(fsm);
        }
        protected internal override void OnUpdate(ShootTankController fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            SearchTarget(fsm);
            if (fsm.target != null) ChangeState<ShootTankAttackState>(fsm);

            Patrol(fsm);
        }
        private void SearchTarget(ShootTankController fsm)
        {
            var colliders = Physics.OverlapSphere(fsm.transform.position, searchRange, 1 << 9);
            foreach (var collider in colliders)
            {
                if (Vector3.Angle(collider.transform.position - fsm.transform.position, fsm.transform.forward) <= searchAngle / 2)
                {
                    if (fsm.target == null) fsm.target = collider.transform;
                    else fsm.target =
                            Vector3.SqrMagnitude(fsm.target.position - fsm.transform.position) < Vector3.SqrMagnitude(collider.transform.position - fsm.transform.position) ?
                            fsm.target : collider.transform;
                }
            }
        }
        private void Patrol(ShootTankController fsm)
        {


            timeCnt -= Time.deltaTime;
            if (timeCnt <= 0)
            {
                timeCnt = refreshTime;
                fsm.agent.SetDestination(
                    new Vector3(Random.Range(-patrolRadius, patrolRadius),
                    0,
                    Random.Range(-patrolRadius, patrolRadius))
                    );
            }
        }
    }
}
