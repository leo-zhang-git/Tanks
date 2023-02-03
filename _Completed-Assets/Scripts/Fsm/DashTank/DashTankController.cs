using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public class DashTankController : Fsm<DashTankController>
    {
        public NavMeshAgent agent;
        public Transform target;
        protected override void Awake()
        {
            base.Awake();
            agent= GetComponent<NavMeshAgent>();
            
        }
        override protected void OnEnable()
        {
            base.OnEnable();
            target = null;
            if(Owner) ChangeState<DashTankPatrolState>();
        }
        void Start()
        {
            FsmInit(
                new DashTankPatrolState(),
                new DashTankDashState());
            StartState<DashTankPatrolState>();
        }
        public override void Pause(float time)
        {
            base.Pause(time);
            agent.isStopped= true;
        }
        public override void Continue()
        {
            base.Continue();
            agent.isStopped= false;
        }
    }
}
