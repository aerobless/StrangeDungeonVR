using UnityEngine;

namespace SixtyMeters.logic.utilities
{
    public class PersistentObject : MonoBehaviour
    {
        private bool _isPersistent;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void MakePersistent()
        {
            if (!_isPersistent)
            {
                // This gameObject does not get deleted when individual rooms are deleted.
                var persistentObjectStorage = GameObject.Find("PersistentObjectStorage");
                transform.parent = persistentObjectStorage.transform;
                _isPersistent = true;
            }
        }
    }
}