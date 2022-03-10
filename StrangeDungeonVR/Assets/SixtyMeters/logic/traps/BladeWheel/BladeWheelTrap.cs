using System.Collections;
using UnityEngine;

namespace SixtyMeters.logic.traps.BladeWheel
{
    public class BladeWheelTrap : MonoBehaviour, ITrap
    {
        public GameObject projectileBladeWheel;
        public AudioSource audioSource;
        public AudioClip spawnBladeSound;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void TriggerTrap()
        {
            audioSource.PlayOneShot(spawnBladeSound);
            StartCoroutine(LaunchProjectile(1.85f));
        }

        private IEnumerator LaunchProjectile(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            Instantiate(projectileBladeWheel, gameObject.transform);
        }

        public void ResetTrap()
        {
            throw new System.NotImplementedException();
        }
    }
}