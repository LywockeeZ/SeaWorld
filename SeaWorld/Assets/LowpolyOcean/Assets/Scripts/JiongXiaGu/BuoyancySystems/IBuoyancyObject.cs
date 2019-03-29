using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    public interface IBuoyancyObject
    {
        void UpdateBuoyancyPoints();
        void AddForce(IBuoyancyData data);
        void AddForce(BuoyantForce force);
        void AddForce(Vector3 force);
    }
}
