using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public class ShootTankController : Fsm<ShootTankController>
    {
        public NavMeshAgent agent;
        public Transform target;
        public TankShooting tankShooting;
        public bool IsPause;

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            tankShooting= GetComponent<TankShooting>();
        }
        protected override void OnEnable()
        {
            target = null;
            if (Owner) ChangeState<ShootTankPatrolState>();
        }
        void Start()
        {
            FsmInit(
                new ShootTankPatrolState(),
                new ShootTankAttackState());
            StartState<ShootTankPatrolState>();
        }

        public override void Pause(float time)
        {
            base.Pause(time);
            agent.isStopped = true;
            IsPause= true;
        }
        public override void Continue()
        {
            base.Continue();
            agent.isStopped = false;
            IsPause= false;
        }
    }
}
