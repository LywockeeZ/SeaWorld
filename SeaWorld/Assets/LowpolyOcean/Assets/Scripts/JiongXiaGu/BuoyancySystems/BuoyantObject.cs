using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    public class BuoyantObject : MonoBehaviour, IBuoyancyObject
    {
        protected BuoyantObject()
        {
        }

        [SerializeField] private Rigidbody physicObject;
        protected List<IBuoyancyPoint> buoyancyPoints;

        protected virtual void Start()
        {
            buoyancyPoints = new List<IBuoyancyPoint>();
        }

        protected virtual void OnEnable()
        {
            BuoyancySystem.Current.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            BuoyancySystem.Current.Unsubscribe(this);
        }

        public virtual void UpdateBuoyancyPoints()
        {
            buoyancyPoints.Clear();
            GetComponentsInChildren(buoyancyPoints);
        }

        public virtual void AddForce(IBuoyancyData data)
        {
            foreach (var buoyancyPoint in buoyancyPoints)
            {
                buoyancyPoint.AddForce(data, this);

                //BuoyantForce force;
                //if (buoyancyPoint.GetForce(data, out force))
                //{
                //    physicObject.AddForceAtPosition(force.Force, force.Position, ForceMode.Force);
                //}
            }
        }

        public void AddForce(BuoyantForce force)
        {
            physicObject.AddForceAtPosition(force.Force, force.Position, ForceMode.Force);
        }

        public void AddForce(Vector3 force)
        {
            physicObject.AddForce(force);
        }
    }
}
