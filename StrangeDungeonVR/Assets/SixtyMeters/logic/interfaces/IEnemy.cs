using UnityEngine;

namespace SixtyMeters.logic.interfaces
{
    public interface IEnemy
    {
        bool IsAlive();

        Vector3 GetPosition();
    }
}