using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public abstract class FsmState<T> where T : Fsm<T>
    {
        /// <summary>
        /// ��ʼ������״̬��״̬�������ʵ����
        /// </summary>
        public FsmState()
        {
        }

        /// <summary>
        /// ����״̬��״̬��ʼ��ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnInit(T fsm)
        {
        }

        /// <summary>
        /// ����״̬��״̬����ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnEnter(T fsm)
        {
            fsm.curStateName = this.GetType().Name; 
        }

        /// <summary>
        /// ����״̬��״̬��ѯʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
        protected internal virtual void OnUpdate(T fsm, float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// ����״̬��״̬�뿪ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="isShutdown">�Ƿ��ǹر�����״̬��ʱ������</param>
        protected internal virtual void OnLeave(T fsm, bool isShutdown)
        {
        }

        /// <summary>
        /// ����״̬��״̬����ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnDestroy(T fsm)
        {
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ�л���������״̬��״̬���͡�</typeparam>
        /// <param name="fsm">����״̬�����á�</param>
        protected void ChangeState<TState>(T fsm) where TState : FsmState<T>
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid.");
            }
            fsm.ChangeState<TState>();
        }
    }
}
