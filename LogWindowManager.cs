using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Logviewer.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class LogWindowManager : MonoBehaviour
    {
        [SerializeField]
        public GameObject EnableWrapping; // toggle

        [SerializeField]
        public GameObject Filter; // inputfield

        [SerializeField]
        public GameObject Text; // text
    }
}
