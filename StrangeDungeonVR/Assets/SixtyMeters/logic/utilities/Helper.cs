using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.logic.interfaces;
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

        public static IEnumerator MovePosition(GameObject objectToMove, Vector3 targetPosition, float duration)
        {
            float time = 0;
            var startPosition = objectToMove.transform.position;
            while (time < duration)
            {
                objectToMove.GetRigidbody().MovePosition(Vector3.Lerp(startPosition, targetPosition, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
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

        public static IEnumerator LerpPosition(Transform objectToMove, Transform target, float duration, Action doAfter)
        {
            float time = 0;
            var startPosition = objectToMove.localPosition;
            while (time < duration)
            {
                var smoothStep = Mathf.SmoothStep(0f, 1f, time / duration);
                objectToMove.localPosition = Vector3.Lerp(startPosition, target.localPosition, smoothStep);
                time += Time.deltaTime;
                yield return null;
            }

            objectToMove.localPosition = target.localPosition;
            doAfter?.Invoke();
        }

        public static IEnumerator LerpWorldPosition(Transform objectToMove, Transform target, float duration,
            Action doAfter)
        {
            float time = 0;
            var startPosition = objectToMove.position;
            while (time < duration)
            {
                var smoothStep = Mathf.SmoothStep(0f, 1f, time / duration);
                objectToMove.position = Vector3.Lerp(startPosition, target.position, smoothStep);
                time += Time.deltaTime;
                yield return null;
            }

            objectToMove.position = target.position;
            doAfter?.Invoke();
        }

        public static IEnumerator LerpPosition(Transform objectToMove, Vector3 targetPosition, float duration)
        {
            return LerpPosition(objectToMove, targetPosition, duration, NoAction());
        }

        public static IEnumerator LerpRotation(Transform objectToRotate, Quaternion targetRotation, float duration,
            Action doAfter)
        {
            float time = 0;
            var startRotation = objectToRotate.localRotation;
            while (time < duration)
            {
                objectToRotate.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            objectToRotate.localRotation = targetRotation;
            doAfter?.Invoke();
        }


        public static IEnumerator PlaySound(AudioSource audioSource, List<AudioClip> audioClips, Action doAfter)
        {
            var randomClip = GETRandomFromList(audioClips);
            return PlaySound(audioSource, randomClip, randomClip.length, doAfter);
        }

        public static void PlayRandomIfExists(AudioSource audioSource, List<AudioClip> audioClips)
        {
            if (audioClips.Count > 0)
            {
                audioSource.PlayOneShot(Helper.GETRandomFromList(audioClips));
            }
        }

        public static IEnumerator PlaySound(AudioSource audioSource, AudioClip audioClip, Action doAfter)
        {
            return PlaySound(audioSource, audioClip, audioClip.length, doAfter);
        }

        public static IEnumerator PlaySound(AudioSource audioSource, List<AudioClip> audioClips, float waitDuration,
            Action doAfter)
        {
            var randomClip = GETRandomFromList(audioClips);
            return PlaySound(audioSource, randomClip, waitDuration, doAfter);
        }

        public static IEnumerator PlaySound(AudioSource audioSource, AudioClip audioClip, float waitDuration,
            Action doAfter)
        {
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(waitDuration);
            doAfter?.Invoke();
        }

        public static IEnumerator PlayParticles(ParticleSystem effect, float duration, Action doAfter)
        {
            effect.Play(true);
            yield return new WaitForSeconds(duration);
            effect.Stop(true);
            doAfter?.Invoke();
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

        public static T CreateDeepCopy<T>(T obj)
        {
            using var ms = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            return (T) formatter.Deserialize(ms);
        }

        public static IChanceTicket GETRandomWinner(List<IChanceTicket> ticketObjects)
        {
            List<IChanceTicket> lottery = new();
            ticketObjects.ForEach(entry =>
            {
                for (int i = 0; i < entry.GetTicketCount(); i++)
                {
                    lottery.Add(entry);
                }
            });
            return GETRandomFromList(lottery);
        }
    }
}