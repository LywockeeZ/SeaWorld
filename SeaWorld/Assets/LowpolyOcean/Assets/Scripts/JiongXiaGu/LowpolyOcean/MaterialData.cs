using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    public class MaterialData : IEquatable<MaterialData>, IEquatable<Material>
    {
        public Material Material;
        public int UpdaterHash { get; set; }
        public int Version { get; set; }

        public MaterialData()
        {
        }

        public MaterialData(Material material)
        {
            Material = material;
        }

        public override bool Equals(object obj)
        {
            return obj is MaterialData && Equals((MaterialData)obj);
        }

        public bool Equals(MaterialData other)
        {
            return EqualityComparer<Material>.Default.Equals(Material, other.Material) &&
                   UpdaterHash == other.UpdaterHash &&
                   Version == other.Version;
        }

        public override int GetHashCode()
        {
            var hashCode = -2054894515;
            hashCode = hashCode * -1521134295 + EqualityComparer<Material>.Default.GetHashCode(Material);
            hashCode = hashCode * -1521134295 + UpdaterHash.GetHashCode();
            hashCode = hashCode * -1521134295 + Version.GetHashCode();
            return hashCode;
        }

        public bool Equals(Material other)
        {
            return EqualityComparer<Material>.Default.Equals(Material, other);
        }

        public static implicit operator Material(MaterialData data)
        {
            return data.Material;
        }

        public static bool operator ==(MaterialData data1, MaterialData data2)
        {
            return data1.Equals(data2);
        }

        public static bool operator !=(MaterialData data1, MaterialData data2)
        {
            return !(data1 == data2);
        }

        public static bool operator ==(MaterialData data1, Material data2)
        {
            return data1.Equals(data2);
        }

        public static bool operator !=(MaterialData data1, Material data2)
        {
            return !(data1 == data2);
        }
    }
}
