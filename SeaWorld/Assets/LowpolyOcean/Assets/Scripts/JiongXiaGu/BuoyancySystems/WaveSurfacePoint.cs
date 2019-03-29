using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    [DisallowMultipleComponent]
    public class WaveSurfacePoint : MonoBehaviour, IBuoyancyPoint
    {
        [SerializeField]
        private float offset;

        public void AddForce(IBuoyancyData data, IBuoyancyObject buoyancyObject)
        {
            Vector3 position = transform.position;
            var d = data.DistanceToSurface(position);
            position.y -= d;
            transform.position = position;
        }
    }
}
