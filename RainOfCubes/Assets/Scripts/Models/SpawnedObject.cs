using System.Collections;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public abstract class SpawnedObject : MonoBehaviour, IExplodable
    {
        [SerializeField] private float _minLifetime = 2f;
        [SerializeField] private float _maxLifetime = 5f;

        protected Renderer Renderer { get; private set; }
        protected Coroutine Coroutine { get; set; }

        public Rigidbody Rigidbody { get; private set; }
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Renderer = GetComponent<Renderer>();
        }

        protected virtual void OnDisable()
        {
            if (Coroutine != null)
                StopCoroutine(Coroutine);
        }

        protected abstract IEnumerator CalculateLifetime();

        protected float GetLifetime() => Random.Range(_minLifetime, _maxLifetime + 1f);
    }
}