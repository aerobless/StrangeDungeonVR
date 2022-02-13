using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SixtyMeters.logic.player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SixtyMeters.logic.utilities
{
    public static class Helper
    {
        /**
         * Returns a random entry from the list.
         */
        public static T GETRandomFromList<T>(IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        // Lerp stuff
        public static IEnumerator LerpPosition(Transform objectToMove, Vector3 targetPosition, float duration,
            Action doAfter)
        {
            float time = 0;
            var startPosition = objectToMove.localPosition;
            while (time < duration)
            {
                objectToMove.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            objectToMove.localPosition = targetPosition;
            doAfter?.Invoke();
        }

        public static IEnumerator LerpPosition(Transform objectToMove, Vector3 targetPosition, float duration)
        {
            return LerpPosition(objectToMove, targetPosition, duration, NoAction());
        }
        
        public static IEnumerator Wait(float timeToWait, Action doAfter)
        {
            yield return new WaitForSeconds(timeToWait);
            doAfter?.Invoke();
        }

        public static IEnumerator Wait(float timeToWait)
        {
            return Wait(timeToWait, NoAction());
        }

        public static Action NoAction()
        {
            return () => { };
        }
    }
}