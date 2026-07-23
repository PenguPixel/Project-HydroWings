using UnityEngine.Splines;

namespace Interfaces
{
    public interface IPoolableEnemy
    {
        void OnSpawn(SplineContainer spline);
        void OnDespawn();
    }
}