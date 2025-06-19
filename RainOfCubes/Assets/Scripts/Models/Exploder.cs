using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Models
{
    public class Exploder : MonoBehaviour
    {
        [SerializeField] private float _baseExplosionForce = 3000;
        [SerializeField] private float _baseExplosionRadius = 20;

        public void Explode(Bomb explosionBomb)
        {
            foreach (var explodableObject in GetExplodableObjects(explosionBomb)
                         .Select(target => target.Rigidbody))
            {
                explodableObject.AddExplosionForce(
                    _baseExplosionForce,
                    explosionBomb.transform.position,
                    _baseExplosionRadius);
            }
        }

        private List<IExplodable> GetExplodableObjects(Bomb explosionBomb)
        {
            var targetColliders = Physics.OverlapSphere(explosionBomb.transform.position, _baseExplosionRadius);
            List<IExplodable> targets = new();

            foreach (var targetCollider in targetColliders)
            {
                if (targetCollider.TryGetComponent(out IExplodable target))
                {
                    targets.Add(target);
                }
            }

            return targets;
        }
    }
}