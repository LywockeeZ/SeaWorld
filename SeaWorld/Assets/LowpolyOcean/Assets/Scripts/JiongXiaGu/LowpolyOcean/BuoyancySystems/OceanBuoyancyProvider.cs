using JiongXiaGu.BuoyancySystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [DisallowMultipleComponent]
    public sealed class OceanBuoyancyProvider : MonoBehaviour, IBuoyancyData
    {
        private OceanBuoyancyProvider()
        {
        }

        public OceanSettings OceanSettings;
        [SerializeField] private float density = 1;
        [SerializeField] private Vector3 directionOfFlow;

        public float Density
        {
            get { return density; }
            set { density = value; }
        }

        private void OnEnable()
        {
            BuoyancySystem.SetBuoyancyData(this);
        }

        private void OnDisable()
        {
            BuoyancySystem.RemoveBuoyancyData(this);
        }

        float IBuoyancyData.DistanceToSurface(Vector3 postion)
        {
            float height = transform.position.y;
            height += GetWaveOffset(postion);
            return postion.y - height;
        }

        public Vector3 GetDirectionOfFlow(Vector3 position)
        {
            return directionOfFlow;
        }

        private float GetWaveOffset(Vector3 pos)
        {
            if (OceanSettings == null)
                return 0;

            return OceanSettings.GetVertexHeight(pos);
        }
    }
}
