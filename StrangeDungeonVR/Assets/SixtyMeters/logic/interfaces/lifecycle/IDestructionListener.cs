using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.interfaces.lifecycle
{
    public interface IDestructionListener
    {
        public void ObjectDestroyed(GameObject trackedObject);
    }
}