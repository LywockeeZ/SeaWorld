using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    public sealed class SphereBuoyancy : MonoBehaviour, IBuoyancyPoint
    {
        private SphereBuoyancy()
        {
        }

        [SerializeField] private float intensity = 1;
        [SerializeField] private float radius = 0.5f;

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public void AddForce(IBuoyancyData data, IBuoyancyObject buoyancyObject)
        {
            var pos = transform.position;
            var distanceToSurface = data.DistanceToSurface(pos);
            float radius = this.radius * BuoyancyHelper.GetRadiusScaleFactor(transform.lossyScale);
            if (distanceToSurface > radius)
            {
                return;
            }
            else
            {
                float scale = (radius - distanceToSurface) / (radius * 2);
                scale = Mathf.Clamp01(scale);
                Vector3 force = data.Density * -Physics.gravity * scale * intensity;
                buoyancyObject.AddForce(new BuoyantForce(pos, force));
            }
        }

        public bool GetForce(IBuoyancyData buoyancy, out BuoyantForce force)
        {
            var pos = force.Position = transform.position;
            var distanceToSurface = buoyancy.DistanceToSurface(pos);
            float radius = this.radius * BuoyancyHelper.GetRadiusScaleFactor(transform.lossyScale);
            if (distanceToSurface > radius)
            {
                force.Force = Vector3.zero;
                return false;
            }
            else
            {
                float scale = (radius - distanceToSurface) / (radius * 2);
                scale = Mathf.Clamp01(scale);
                force.Force = buoyancy.Density * -Physics.gravity * scale * intensity;
                return true;
            }
        }


        public static Color BoundColor { get; set; } = Color.blue;
        public static Color ForceColor { get; set; } = Color.red;
        public static bool IsDrawWire { get; set; }

        private void OnDrawGizmos()
        {
            if (IsDrawWire)
            {
                DrawWire();
            }
        }

        private void OnDrawGizmosSelected()
        {
            DrawWire();
        }

        private void DrawWire()
        {
            Gizmos.color = BoundColor;
            Gizmos.DrawWireSphere(transform.position, radius * BuoyancyHelper.GetRadiusScaleFactor(transform.lossyScale));

            Gizmos.color = ForceColor;
            Gizmos.DrawRay(transform.position, -Physics.gravity.normalized * radius * 2);
        }
    }
}
