using SixtyMeters.logic.item;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    public class LockedChest : MonoBehaviour
    {
        // Components
        public GameObject chestLid;
        public AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            var key = other.GetComponent<Key>();
            if (key)
            {
                //TODO: verify that it's the right key
                var targetRotation = Quaternion.Euler(-120, 0, 0);
                StartCoroutine(Helper.LerpRotation(chestLid.transform, targetRotation, 2f, () => { }));
            }
        }
    }
}