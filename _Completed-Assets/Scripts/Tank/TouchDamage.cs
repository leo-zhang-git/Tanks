using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class TouchDamage : MonoBehaviour
    {
        float damage = 10f;
        TankHealth selfHealth;
        private void Start()
        {
            selfHealth = transform.GetComponent<TankHealth>();
            if (!selfHealth) Debug.LogError("selfHealth is null");
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!selfHealth || collision.collider.tag == "Terra") return;

            var tankHealth = collision.collider.GetComponent<TankHealth>();
            if (tankHealth)
            {
                tankHealth.TakeDamage(damage);
            }
            selfHealth.TakeDamage(damage);
        }
    }
}
