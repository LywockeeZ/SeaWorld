using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    [Serializable]
    public class HorizonBuoyancyProvider : IBuoyancyData
    {
        [SerializeField]
        private float density = 1;
        [SerializeField]
        private float height = 0;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Density
        {
            get { return density; }
            set { density = value; }
        }

        public float DistanceToSurface(Vector3 postion)
        {
            return postion.y - Height;
        }

        public Vector3 GetDirectionOfFlow(Vector3 position)
        {
            return Vector3.zero;
        }
    }
}
