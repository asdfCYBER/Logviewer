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

        public List<Log> FilteredLogs { get; private set; } = new List<Log>();

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

        public void MessageReceived(string message, string stacktrace, LogType type)
        {
            Log log = new Log(message, type);
            Logs.Add(log);
            if (log.MatchFilter(_filter))
                FilteredLogs.Add(log);

            if (_logWindow?.LogView?.content == null || !log.MatchFilter(_filter)) return;
            _logWindow.AddLog(log.ToString());
        }

        public void ShowLogs(List<Log> logs)
        {
            _logWindow.Clear();
            foreach (Log log in logs)
                _logWindow.AddLog(log.ToString());
        }

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
                _panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, -400);
                _logWindow = _panel.GetComponent<LogWindowManager>();
                _logWindow.AddExternalListeners(delegate { _filter = _logWindow.Filter.text; ShowLogs(FilterLogs()); }, delegate { });
            }

            _panel.SetActive(true);

            if (string.IsNullOrEmpty(_filter))
                ShowLogs(Logs);
            else
            {
                ShowLogs(FilterLogs());
            }
        }

        public void ToggleWindow(InputAction.CallbackContext context)
        {
            if (_panel != null)
                _panel.SetActive(!_panel.activeSelf);
            else
                EnableWindow();
        }

        public List<Log> FilterLogs()
        {
            return Logs.Where(log => log.MatchFilter(_filter)).ToList();
        }

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
