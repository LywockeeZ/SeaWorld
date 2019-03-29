using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{


    public struct BuoyantForce : IEquatable<BuoyantForce>
    {
        public Vector3 Position;
        public Vector3 Force;

        public BuoyantForce(Vector3 position, Vector3 force)
        {
            Position = position;
            Force = force;
        }

        public override bool Equals(object obj)
        {
            return obj is BuoyantForce && Equals((BuoyantForce)obj);
        }

        public bool Equals(BuoyantForce other)
        {
            return Position.Equals(other.Position) &&
                   Force.Equals(other.Force);
        }

        public override int GetHashCode()
        {
            var hashCode = 101205726;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Force);
            return hashCode;
        }

        public static bool operator ==(BuoyantForce force1, BuoyantForce force2)
        {
            return force1.Equals(force2);
        }

        public static bool operator !=(BuoyantForce force1, BuoyantForce force2)
        {
            return !(force1 == force2);
        }
    }
}
