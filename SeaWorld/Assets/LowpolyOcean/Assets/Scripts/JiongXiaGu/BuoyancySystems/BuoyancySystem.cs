using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    public enum UpdateMode
    {
        Manual = 0,
        Auto,
    }

    internal sealed class BuoyancyUpdater : MonoBehaviour
    {
        private BuoyancyUpdater()
        {
        }

        public BuoyancySystem System { get; set; }

        private void FixedUpdate()
        {
            if (System.UpdateMode == UpdateMode.Auto)
            {
                System.Update();
            }
        }
    }

    public sealed class BuoyancySystem
    {
        #region Singleton

        private static Lazy<BuoyancySystem> current = new Lazy<BuoyancySystem>(delegate()
        {
            var system = new BuoyancySystem()
            {
                BuoyancyData = new HorizonBuoyancyProvider(),
                UpdateMode = UpdateMode.Auto,
            };
            system.Update();
            return system;
        });
        public static BuoyancySystem Current => current.Value;

        public static void SetBuoyancyData(IBuoyancyData buoyancy)
        {
            Current.BuoyancyData = buoyancy;
        }

        public static void RemoveBuoyancyData(IBuoyancyData buoyancy)
        {
            Current.BuoyancyData = null;
        }

        #endregion

        private readonly List<IBuoyancyObject> buoyancyObjects = new List<IBuoyancyObject>();
        public IReadOnlyCollection<IBuoyancyObject> BuoyancyObjects => buoyancyObjects;
        private BuoyancyUpdater buoyancyUpdater;
        public IBuoyancyData BuoyancyData { get; set; }
        public UpdateMode UpdateMode { get; set; }

        private BuoyancyUpdater CreateUpdater()
        {
            if (buoyancyUpdater == null)
            {
                buoyancyUpdater = new GameObject(nameof(BuoyancyUpdater) + " " + GetHashCode(), typeof(BuoyancyUpdater)).GetComponent<BuoyancyUpdater>();
                buoyancyUpdater.System = this;
                UnityEngine.Object.DontDestroyOnLoad(buoyancyUpdater.gameObject);
                buoyancyUpdater.hideFlags = HideFlags.HideAndDontSave;
            }
            return buoyancyUpdater;
        }

        private void DestoryUpdater()
        {
            if (buoyancyUpdater != null)
            {
                UnityEngine.Object.Destroy(buoyancyUpdater.gameObject);
                buoyancyUpdater = null;
            }
        }

        public void Subscribe(IBuoyancyObject buoyancyObject)
        {
            if (buoyancyObject == null)
                throw new ArgumentNullException(nameof(buoyancyObject));

            if (!buoyancyObjects.Contains(buoyancyObject))
            {
                buoyancyObjects.Add(buoyancyObject);
                CreateUpdater();
            }
        }

        public void Unsubscribe(IBuoyancyObject buoyancyObject)
        {
            if (buoyancyObject == null)
                throw new ArgumentNullException(nameof(buoyancyObject));

            buoyancyObjects.Remove(buoyancyObject);
            if (buoyancyObjects.Count == 0)
            {
                DestoryUpdater();
            }
        }

        public void Update()
        {
            if (BuoyancyData == null)
                return;

            foreach (var buoyancyObject in buoyancyObjects)
            {
                buoyancyObject.UpdateBuoyancyPoints();
                buoyancyObject.AddForce(BuoyancyData);
            }
        }
    }
}
