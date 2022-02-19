using System;
using System.Collections;
using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using TMPro;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class DamageText : MonoBehaviour
    {
        public TextMeshPro textMesh;

        // Settings
        public float moveUpHeight = 3f;
        public float lerpDuration = 2f;

        // Internals
        private Transform _playerCameraTransform;

        // Start is called before the first frame update
        void Start()
        {
            _playerCameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            RotateTextToFacePlayer();
        }


        public void SetDamageText(int dmg)
        {
            SetDamageText(dmg + "");
        }

        public void SetDamageText(string text)
        {
            textMesh.text = text;
            var endPosition = transform.position + new Vector3(0, moveUpHeight, 0);
            StartCoroutine(Helper.LerpPosition(transform, endPosition, lerpDuration, DestroyText(gameObject)));
            StartCoroutine(LerpTextAlpha(textMesh, 0, 2));
        }

        private void RotateTextToFacePlayer()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _playerCameraTransform.position);
        }

        private static Action DestroyText(GameObject gameObject)
        {
            return () => { Destroy(gameObject); };
        }

        private static IEnumerator LerpTextAlpha(TextMeshPro text, float targetValue, float duration)
        {
            float time = 0;
            var startValue = text.alpha;
            while (time < duration)
            {
                text.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            text.alpha = targetValue;
        }
    }
}