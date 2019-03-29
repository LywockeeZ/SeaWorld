using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// use to initialize ocean setting, only one instance is allow in scene
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu(EditorModeHelper.AddComponentMenuNameRoot + nameof(OceanController))]
    public sealed class OceanController : MonoBehaviour
    {
        private OceanController()
        {
        }

        [SerializeField]
        private ProjectSettings projectSettings;
        public ProjectSettings ProjectSettings => projectSettings;

        private void Awake()
        {
            projectSettings?.SetToCurrentSettings();
        }

        private void OnValidate()
        {
            projectSettings?.SetToCurrentSettings();
        }
    }
}
