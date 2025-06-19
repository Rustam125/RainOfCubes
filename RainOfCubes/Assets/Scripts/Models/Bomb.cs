using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class Bomb: MonoBehaviour, IExplodable
    {
        private const float MaxTransparencyValue = 1;
        private const float MinTransparencyValue = 0;

        [SerializeField] private int _minLifetime = 2;
        [SerializeField] private int _maxLifetime = 5;
        
        private Renderer _renderer;
        private Material _material;
        private Rigidbody _rigidbody;
        private Coroutine _coroutine;
        
        public event Action<Bomb> Released;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(Destroy());
        }
        
        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
        
        public void SetDefaultTransparency() => SetTransparency(MaxTransparencyValue);
        
        private void SetTransparency(float value)
        {
            var color = _material.color;
            color.a = value;
            _material.color = color;
        }

        private IEnumerator Destroy()
        {
            var lifeTime = GetLifetime();
            float timer = 0;

            while (timer < lifeTime)
            {
                timer += Time.deltaTime;
                SetTransparency(1 - timer / lifeTime);
                yield return null;
            }

            SetTransparency(MinTransparencyValue);
            Released?.Invoke(this);
        }

        private float GetLifetime() =>
            Random.Range(_minLifetime, _maxLifetime + 1f);
    }
}