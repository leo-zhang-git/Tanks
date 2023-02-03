using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public abstract class Fsm<T> : MonoBehaviour where T : Fsm<T>
    {
        public T Owner { get; private set; }
        public FsmState<T> CurState
        {
            get { return curState; }
        }

        [SerializeField]
        public string curStateName;

        protected FsmState<T> curState;
        protected Dictionary<Type, FsmState<T>> stateDic;
        private bool m_IsDestroyed;
        protected bool m_IsPaused;
        private float m_PauseTime;

        protected void StartState<TState>() where TState : FsmState<T>
        {
            Owner = this as T;
            curState = stateDic[typeof(TState)];
            curState.OnEnter(Owner);
        }
        protected internal void ChangeState<TState>() where TState : FsmState<T>
        {
            if (stateDic[typeof(TState)] == null) Debug.LogError(typeof(T) + " State invalid");
            CurState.OnLeave(Owner, false);
            curState = stateDic[typeof(TState)];
            curState.OnEnter(Owner);
        }

        protected void FsmInit(params FsmState<T>[] states)
        {
            stateDic = new Dictionary<Type, FsmState<T>>();
            foreach (var state in states)
            {
                stateDic[state.GetType()] = state;
                state.OnInit(Owner);
            }
            m_IsDestroyed = false;
        }

        protected virtual void Awake()
        {
        }
        protected virtual void OnEnable()
        {

        }
        // Update is called once per frame
        protected virtual void Update()
        {
            if(CurState == null) return;

            if (m_IsPaused) m_PauseTime -= Time.deltaTime;
            if (m_PauseTime <= 0) Continue();
            if (m_IsPaused) return;

            CurState.OnUpdate(Owner, Time.time, Time.realtimeSinceStartup);
        }
        protected virtual void OnDisable()
        {
            Destroy(GetComponent<T>());
        }
        protected virtual void OnDestroy()
        {
            Owner = null;
            if (m_IsDestroyed) return;
        }
        protected virtual void Clear()
        {
            if (m_IsDestroyed) Debug.LogError("fsm already Destroyed");
            curState = null;
            foreach(var state in stateDic.Values)
            {
                state.OnDestroy(Owner);
            }
        }
        public virtual void Pause(float time)
        {
            m_IsPaused = true;
            m_PauseTime = time;
        }
        public virtual void Continue()
        {
            m_IsPaused = false;
        }
    }
}
