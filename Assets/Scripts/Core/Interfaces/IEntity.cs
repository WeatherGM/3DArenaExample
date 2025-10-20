

using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IEntity : IDamageable
    {
        void EndLiving();
        void Move(Vector3 forward);
        void StartLiving();
    }
}