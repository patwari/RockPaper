using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils {
    public class HeartbeatScaler : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void Awake() {
            if (target == null) target = gameObject;
        }

        private void Start() {
            DoHeartBeat();
        }

        private void DoHeartBeat() {
            // Scale up and down to mimic a heartbeat
            Sequence heartbeatSequence = DOTween.Sequence();
            heartbeatSequence.Append(target.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f)) // Scale up quickly
                             .Append(target.transform.DOScale(Vector3.one, 0.2f)) // Scale down quickly
                             .Append(target.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f)) // Scale up slightly slower
                             .Append(target.transform.DOScale(Vector3.one, 0.3f)) // Scale down slightly slower
                             .SetLoops(-1, LoopType.Restart); // Repeat indefinitely


            var image = target.GetComponent<Image>();
            if (image == null) {
                // Change color to a random color
                Sequence colorSequence = DOTween.Sequence();
                colorSequence.Append(image.DOColor(GetRandomColor(), 0.2f))
                             .Append(image.DOColor(GetRandomColor(), 0.2f))
                             .Append(image.DOColor(GetRandomColor(), 0.3f))
                             .Append(image.DOColor(GetRandomColor(), 0.3f))
                             .SetLoops(-1, LoopType.Restart); // Repeat indefinitely
            }
        }

        private Color GetRandomColor() {
            return new Color(Random.value, Random.value, Random.value);
        }
    }
}