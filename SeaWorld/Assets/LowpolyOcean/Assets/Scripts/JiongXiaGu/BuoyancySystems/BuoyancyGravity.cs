using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    /// <summary>
    /// Add force in the direction of gravity at the component position
    /// </summary>
    public sealed class BuoyancyGravity : MonoBehaviour, IBuoyancyPoint
    {
        private BuoyancyGravity()
        {
        }

        [SerializeField] private float intensity = 1;

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public void AddForce(IBuoyancyData data, IBuoyancyObject buoyancyObject)
        {
            buoyancyObject.AddForce(Physics.gravity * intensity);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
