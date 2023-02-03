using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class DropBase : MonoBehaviour
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
