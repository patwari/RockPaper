using System;
using System.Collections.Generic;
using UnityEngine;

/**
This is a separate file especially for BGM.
Since BGM are large in size, we load them using addressable (for better memory management).
Process: AudioSource items are still attached to BGM, but without audioClips > 
- When we need to play a BGM, we simply async load the audioClip > and attach it to the AudioSource and then > PLAY.
*/
namespace Sound {
    public partial class SoundManager : MonoBehaviour {
        [Header("BGM")]
        [SerializeField] private AudioSource lobbyBgm;
        [SerializeField] private AudioSource gameplayBgm;

        // Following dictionary is made (on runtime) for easy access. Use them for all bgm related stuff.
        private Dictionary<string, float> defaultBgmVolumes = new Dictionary<string, float>();

        public bool canPlayBgm => DI.di.dataSaver.CanPlayBgm;
        private AudioSource currBgmAudio;

        /// <summary>
        /// Play BGM with the given id. <br/>
        /// Always prefer this method.
        /// </summary>
        /// <param name="bgmId"></param>
        public void PlayBgm(string bgmId) {
            if (currBgmAudio != null) currBgmAudio.mute = true;

            if (bgmId == "lobby") {
                currBgmAudio = lobbyBgm;
            } else if (bgmId == "gameplay") {
                currBgmAudio = gameplayBgm;
            } else {
                currBgmAudio = null;
            }

            if (currBgmAudio != null) {
                currBgmAudio.Play();
                currBgmAudio.mute = !canPlayBgm;
            }

            Debug.Log($"BGM :: PlayBgm :: [{currBgmAudio.name}]");
        }

        private void OnBgmStateChanged() {
            if (currBgmAudio != null)
                currBgmAudio.mute = !canPlayBgm;
        }

        /// <summary>
        /// Stops the currently playing BGM. <br/>
        /// If fadeOut is TRUE, then the BGM will fade out. <br/>
        /// If fadeOut is FALSE, then the BGM will stop instantly.
        /// </summary>
        /// <param name="fadeOut"> default true. FALSE = curr BG will be stopped immediately. </param>
        public void StopBgm() {
            if (currBgmAudio == null || currBgmAudio.clip == null) return;
            currBgmAudio.mute = true;
        }
    }
}