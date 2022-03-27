using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Logviewer.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class LogWindowManager : MonoBehaviour
    {
        [SerializeField]
        public TMP_InputField Filter;

        [SerializeField]
        public ScrollRect LogView;

        [SerializeField]
        public Button Close;

        [SerializeField]
        public TMP_InputField LogInput;

        [SerializeField]
        public GameObject LogTemplate;

        [SerializeField]
        public Button Save;

        public void Start()
        {
            LogInput.onSubmit.AddListener(delegate (string message)
            {
                Debug.Log(message);
                LogInput.text = "";
            });

            Close.onClick.AddListener(delegate
            {
                gameObject.SetActive(false); 
            });
        }

        public void AddExternalListeners(Action<string> onFilterSubmit, Action onSave)
        {
            // unityaction -> normal action
            Filter.onSubmit.AddListener(delegate (string filter) { onFilterSubmit(filter); });
            //Save.onClick.AddListener(delegate { onSave(); });
        }

        /// <summary>
        /// Add a log to the scrollview with message <paramref name="text"/>
        /// </summary>
        /// <param name="text"></param>
        public void AddLog(string text)
        {
            if (LogView?.content == null)
            {
                Debug.LogWarning("[Logview] NullReferenceException in AddLog");
                return;
            }

            GameObject log = Instantiate(LogTemplate, LogView.content.transform, worldPositionStays: false);
            log.GetComponent<TMP_Text>().text = text;
            log.SetActive(true);
        }

        /// <summary>
        /// Remove all logs from the scrollview
        /// </summary>
        public void Clear()
        {
            // destroying items in a loop won't mess anything up here
            // because the objects are actually destroyed after the loop
            foreach (Transform item in LogView.content.transform)
                Destroy(item.gameObject);
        }
    }
}
