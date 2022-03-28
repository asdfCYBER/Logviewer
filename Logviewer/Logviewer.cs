using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.Context;
using Game.Mod;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Utils;
using Logviewer.Unity;

namespace Logviewer
{
    public class Logviewer : AbstractMod
    {
        public override CachedLocalizedString Title => "Logviewer";

        public override CachedLocalizedString Description => "Shows logs in-game";

        public static AssetBundle LogviewerUIAssets { get; } = LoadAssets();

        public List<Log> Logs { get; } = new List<Log>();

        private string _filter;

        private IControllers _controllers;

        private LogWindowManager _logWindow;

        private GameObject _panel;

        private InputAction _openPanelAction;

        public override async Task OnEnable()
        {
            // Subscribe to spam
            Application.logMessageReceivedThreaded += MessageReceived;
            Debug.Log("[Logviewer] Mod has been enabled");

            _openPanelAction = new InputAction("Open Logviewer", binding: "<Keyboard>/f3");
            _openPanelAction.performed += ToggleWindow;
            _openPanelAction.Enable();

            await Task.Yield();
        }

        public override async Task OnDisable()
        {
            // Destroy ui thingy
            _openPanelAction.Disable();
            GameObject.Destroy(_panel);
            Debug.Log("[Logviewer] Mod has been disabled");

            // Unsubscribe from spam
            Application.logMessageReceivedThreaded -= MessageReceived;

            await Task.Yield();
        }

        public override async Task OnContextChanged(IControllers controllers)
        {
            _controllers = controllers;
            Debug.Log($"[Logviewer] Context changed, current mode: {controllers.CurrentMode}");

            await Task.Yield();
        }

        /// <summary>
        /// Add a message to the list of logs and show it on the panel if possible
        /// </summary>
        /// <param name="message">The received message</param>
        /// <param name="stacktrace">Stacktrace if the message is an exception</param>
        /// <param name="type">Type of the message (log, warning, etc.)</param>
        public void MessageReceived(string message, string stacktrace, LogType type)
        {
            Log log = new Log(message, type);
            Logs.Add(log);

            if (log.MatchFilter(_filter) && _logWindow?.LogView?.content != null)
                _logWindow.AddLog(log.ToString());
        }

        /// <summary>
        /// Clear the screen and show the logs
        /// </summary>
        /// <param name="logs"></param>
        public void ShowLogs()
        {
            List<Log> logs = FilterLogs();

            _logWindow.Clear();
            foreach (Log log in logs)
                _logWindow.AddLog(log.ToString());
        }

        public void SaveLogs()
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".log";
            IO.Utils.TrySaveToFile($"{filename}", string.Join("\n", FilterLogs()));
            Debug.Log($"Saved logs as {filename}");
        }

        private List<Log> FilterLogs()
        {
            return Logs.Where(log => log.MatchFilter(_filter)).ToList();
        }

        /// <summary>
        /// Return the transform to attach the panel to
        /// </summary>
        private Transform GetUITransform()
        {
            // find the canvas to attach the panel to
            Transform uiTransform;
            if (_controllers.CurrentMode == GameMode.Menu)
            {
                // if this method ever throws nullrefs it's probably this line
                return uiTransform = SceneManager.GetActiveScene().GetRootGameObjects()
                    .First(obj => obj.name == "Menu").transform.Find("UI");
            }
            else // editor or play
            {
                return _controllers.GameControllers.UiCanvas.gameObject.transform;
            }
        }

        /// <summary>
        /// Instantiate the panel and/or set the panel active
        /// </summary>
        private void EnableWindow()
        {
            Transform parent = GetUITransform();

            if (_panel != null)
            {
                _panel.transform.SetParent(parent);
            }
            else
            {
                GameObject prefab = LogviewerUIAssets.LoadAsset<GameObject>("Logviewer panel");
                _panel = GameObject.Instantiate(prefab, parent, worldPositionStays: false);
                _panel.transform.localScale = new Vector3(2, 2, 2); // don't ask me, this works
                _logWindow = _panel.GetComponent<LogWindowManager>();
                _logWindow.AddExternalListeners(delegate { _filter = _logWindow.Filter.text; ShowLogs(); }, SaveLogs);
            }

            _panel.SetActive(true);
            ShowLogs();
        }

        /// <summary>
        /// Toggle whether or not the panel is active
        /// </summary>
        /// <param name="context"></param>
        public void ToggleWindow(InputAction.CallbackContext context)
        {
            if (_panel != null)
                _panel.SetActive(!_panel.activeSelf);
            else
                EnableWindow();
        }

        /// <summary>
        /// Load the LogviewerUIassets assetbundle
        /// </summary>
        private static AssetBundle LoadAssets()
        {
            // Load assets from file
            AssetBundle assetBundle = IO.Utils.LoadAssetBundle("LogviewerUIassets");
            if (assetBundle == null)
            {
                Debug.LogError("[Logviewer] The LogviewerUIassets file could not be loaded! " +
                    "Logviewer will not be visible.");
                return null;
            }

            return assetBundle;
        }
    }
}
