using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Complete.EneManager;

namespace Complete
{
    public class DropA : DropBase
    {
        protected override void OnTriggerEnter(Collider other)
        {
            EneManager.Instance.FreezingAllEne(5f);
            base.OnTriggerEnter(other);
        }
    }
}
