using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean.DemoTools
{


    public sealed class RuntimeQuit : MonoBehaviour
    {
        private RuntimeQuit()
        {
        }

        public KeyCode quitKey = KeyCode.Escape;
        public GameObject[] uis;

        private void Update()
        {
            if (Input.GetKeyDown(quitKey))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                foreach (var item in uis)
                {
                    item.SetActive(!item.activeInHierarchy);
                }
            }
        }

    }
}
