using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class DropB : DropBase
    {
        protected override void OnTriggerEnter(Collider other)
        {
            TankHealth health = other.GetComponent<TankHealth>();
            if(health == null) { Debug.LogError("TankHealth Component is null"); }
            health.Invincible(5f);
            GetComponent<MeshRenderer>().material.color = Color.red;

            base.OnTriggerEnter(other);
        }
    }
}
