using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    public interface IBuoyancyData
    {
        float Density { get; }

        /// <summary>
        /// Get the distance from the surface, the positive number is above the surface, and the negative number is below the surface;
        /// </summary>
        float DistanceToSurface(Vector3 postion);

        /// <summary>
        /// Return force
        /// </summary>
        Vector3 GetDirectionOfFlow(Vector3 position);
    }
}
