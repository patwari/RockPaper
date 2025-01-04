using System.Collections;
using UnityEngine;

namespace Sound {
    public partial class SoundManager : MonoBehaviour {
        [Header("SFX")]
        [SerializeField] private AudioSource _defaultButtonSound;

        public bool canPlaySfx { get; private set; } = true;
        private string lastSfxPlayed = "";

        private void Awake() {
            if (DI.di.soundManager != null) {
                Destroy(gameObject);
                return;
            }
            DI.di.SetSoundManager(this);
            SubscribeEvents();
        }

        private void SubscribeEvents() { }
        private void UnsubscribeEvents() { }
        private void OnDestroy() => UnsubscribeEvents();

        #region Generic Control Methods
        public bool Exists(string name) {
            if (string.IsNullOrEmpty(name)) return false;
            if (transform.Find(name) == null) return false;
            if (transform.Find(name).GetComponent<AudioSource>() == null) return false;
            return true;
        }

        public void PlaySfx(string name, bool loop = false) {
            if (lastSfxPlayed != name) {
                Debug.Log($"SoundManager :: PlaySfx :: To play soundId = [{name}]");
                lastSfxPlayed = name;
            }

            if (!canPlaySfx) return;
            if (string.IsNullOrEmpty(name)) return;

            var audioSource = transform.Find(name)?.GetComponent<AudioSource>();
            if (audioSource != null) {
                audioSource.Play();
                audioSource.loop = loop;
            }
        }

        public void PlaySfxDelay(string name, bool loop = false, float delay = 0.0f) => StartCoroutine(PlayWithDelay(name, loop, delay));

        private IEnumerator PlayWithDelay(string name, bool loop, float delay) {
            if (string.IsNullOrEmpty(name)) yield break;
            if (delay <= 0f) yield break;
            yield return new WaitForSeconds(delay);
            PlaySfx(name, loop);
        }

        public void StopSfx(string name) {
#if UNITY_EDITOR
            if (!Exists(name)) Debug.LogError($"ERROR :: SoundManager :: StopSfx :: soundId not found :: soundId = [{name}]");
#endif
            if (string.IsNullOrEmpty(name)) return;

            AudioSource audioSource = transform.Find(name)?.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
        }

        public void PlayDefaultButtonClick() {
            if (!canPlaySfx) return;
            _defaultButtonSound.Play();
        }

        #endregion
    }
}