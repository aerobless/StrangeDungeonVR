using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.item;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    public class LockedDoor : MonoBehaviour, IUnlockable, ILockable
    {
        // Components
        public GameObject presentationKey;
        public GameObject door;
        public AudioSource audioSource;

        public List<AudioClip> unlockSounds;

        // Settings
        public float minLockAngle;
        public float maxLockAngle;

        public float minUnlockedAngle;
        public float maxUnlockedAngle;

        // Internal
        private bool _unlocked;
        private HingeJoint _doorHingeJoint;

        // Start is called before the first frame update
        void Start()
        {
            _doorHingeJoint = door.GetComponent<HingeJoint>();
            Lock();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            var key = other.GetComponent<Key>();
            if (key && !_unlocked)
            {
                Destroy(key.gameObject);
                Unlock();
            }
        }

        public void Unlock()
        {
            if (_unlocked) return;
            _unlocked = true;
            presentationKey.SetActive(true);
            StartCoroutine(Helper.LerpRotation(presentationKey.transform, Quaternion.Euler(-90, -90, 90), 0.5f,
                () => { presentationKey.SetActive(false); }));
            StartCoroutine(Helper.PlaySound(audioSource, unlockSounds, 0.5f, () =>
            {
                JointLimits limits = _doorHingeJoint.limits;
                limits.min = minUnlockedAngle;
                limits.max = maxUnlockedAngle;
                _doorHingeJoint.limits = limits;

                var targetRotation = Quaternion.Euler(0, -190, 0);
                StartCoroutine(Helper.LerpRotation(door.transform, targetRotation, 0.5f,
                    () => { }));
            }));
        }

        public void Lock()
        {
            JointLimits limits = _doorHingeJoint.limits;
            limits.min = minLockAngle;
            limits.max = maxLockAngle;
            _doorHingeJoint.limits = limits;
        }
    }
}