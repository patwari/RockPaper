using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using GameConstants;

namespace Utils.UIUtils {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] private Image loadingBar;
        [SerializeField] private GameObject view;
        [SerializeField] private TextMeshProUGUI percText;
        [SerializeField] private TextMeshProUGUI message;

        private Queue<SceneLoadRequest> _sceneToLoadQ = new Queue<SceneLoadRequest>();
        private Coroutine _loadSceneCoroutine = null;

        private void Awake() {
            FillLoader(0f);
            HideLoaderView();
            SubscribeEvents();
        }

        private void SubscribeEvents() {
            EventsModel.LOAD_SCENE += LoadScene;
        }

        private void UnsubscribeEvents() {
            EventsModel.LOAD_SCENE -= LoadScene;
        }

        private void OnDestroy() => UnsubscribeEvents();

        private void ShowLoaderView() => view.SetActive(true);
        private void HideLoaderView() => view.SetActive(false);

        /// <summary>
        /// Load the scene with the given name. <br/>
        /// 1. Shows the Loader View immediately. <br/>
        /// 2. If previous request is already in progress -> Add to a queue, and wait for it to complete. <br/>
        /// 3. else -> Begin the scene loading sequence. <br/>
        /// </summary>
        /// <param name="sceneName"> The exact name from <see cref="Scenes"/> constants </param>
        /// <param name="isAdditive"> Is this scene an additive to the previous one </param>
        /// <param name="msg"> The message to show in the loader View </param>
        private void LoadScene(string sceneName, bool isAdditive, string msg) {
            // Show the Loader view + stop BGM immediately.
            ShowLoaderView();
            DI.di.soundManager.StopBgm();

            // CHECK: If the previous request is already in progress, then add this request to the queue.
            _sceneToLoadQ.Enqueue(new SceneLoadRequest { sceneName = sceneName, isAdditive = isAdditive, msg = msg });
            TryLoadSceneFromQueue(nameof(LoadScene));
        }

        private void TryLoadSceneFromQueue(string caller) {
            // Debug.Log($"SceneLoader :: TryLoadSceneFromQueue :: caller = {caller} :: QueueCount = {_sceneToLoadQ.Count} :: running = {_loadSceneCoroutine != null}");
            if (_sceneToLoadQ.Count == 0) return;

            if (_loadSceneCoroutine == null) {
                SceneLoadRequest req = _sceneToLoadQ.Dequeue();
                _loadSceneCoroutine = StartCoroutine(DoSceneLoadSequence(req.sceneName, req.isAdditive, req.msg));
            }
        }

        // This just unloads the scene. Use it carefully. You need to handle other things on your own.
        private void UnloadScene(string sceneName) => SceneManager.UnloadSceneAsync(sceneName);

        private IEnumerator DoSceneLoadSequence(string sceneName, bool isAdditive, string msg) {
            string prvScene = SceneManager.GetActiveScene().name;

            ShowLoaderView();
            message.SetText(string.IsNullOrEmpty(msg) ? "" : msg);
            FillLoader(0f);

            // Debug.Log($"SceneLoader :: BEGIN :: {prvScene} -> {sceneName} :: {(isAdditive ? "Additive" : "Single")}");
            EventsModel.PRE_SCENE_LOAD_BEGIN?.Invoke(sceneName);

            float startTime = Time.time;
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            op.allowSceneActivation = false;

            // STAGE 1 = actual scene loading
            while (op.progress < 0.9f) {
                FillLoader(op.progress);
                yield return null;
            }

            // STAGE 2 if everything loaded too fast, we still want to show the loader for at least 0.5 sec.
            while (Time.time - startTime < 0.5f) {
                float currPec = loadingBar.fillAmount;
                FillLoader(Mathf.Lerp(currPec, 0.95f, (Time.time - startTime) / 0.5f));
                yield return null;
            }
            if (!op.allowSceneActivation) op.allowSceneActivation = true;
            FillLoader(0.99f);
            yield return null; // safety wait

            FillLoader(1f);
            yield return null;
            HideLoaderView();

            EventsModel.SCENE_LOAD_COMPLETED?.Invoke(sceneName);
            _loadSceneCoroutine = null;
            TryLoadSceneFromQueue(nameof(DoSceneLoadSequence));
        }

        /// <summary>
        /// Fills the loader bar with the given percentage. <br/>
        /// Scene loading happens in 3 phases: <br/>
        /// - <c> 0% - 90% </c> actual scene loading <br/>
        /// - <c> 91% - 95% </c> waiting for respective DI to get ready AND min of 0.5 sec wait <br/>
        /// - <c> 96% - 99% </c> waiting for all Awake() and OnEnable() functions to complete <br/>
        /// - <c> 100% </c> ready to hide <br/>
        /// </summary>
        /// <param name="perc"> perc in range [0f, 1f] </param>
        private void FillLoader(float perc) {
            loadingBar.fillAmount = Mathf.Clamp(perc, 0f, 1f);
            percText.text = (loadingBar.fillAmount * 100).ToString("0") + "%";
        }

        private struct SceneLoadRequest {
            public string sceneName;
            public bool isAdditive;
            public string msg;
        }
    }
}