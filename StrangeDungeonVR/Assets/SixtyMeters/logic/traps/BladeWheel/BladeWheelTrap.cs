using UnityEngine;

namespace SixtyMeters.logic.traps.BladeWheel
{
    public class BladeWheelTrap : MonoBehaviour, ITrap
    {
        public GameObject projectileBladeWheel;

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
            Instantiate(projectileBladeWheel, gameObject.transform);
        }

        public void ResetTrap()
        {
            throw new System.NotImplementedException();
        }
    }
}