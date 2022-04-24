using System.Collections;
using UnityEngine;

namespace SixtyMeters.logic.props.mechanisms
{
    public class FadeMechanism : MonoBehaviour
    {
        public CanvasGroup fadeObject;

        // Settings
        public float beforeFadeValue;
        public float afterFadeValue;
        public float fadeDuration;

        // Start is called before the first frame update
        void Start()
        {
            fadeObject.alpha = beforeFadeValue;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Trigger()
        {
            StartCoroutine(LerpAlpha(beforeFadeValue, afterFadeValue, fadeDuration));
        }

        private IEnumerator LerpAlpha(float startValue, float endValue, float duration)
        {
            float time = 0;
            while (time < duration)
            {
                fadeObject.alpha = Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;

                yield return null;
            }
        }
    }
}