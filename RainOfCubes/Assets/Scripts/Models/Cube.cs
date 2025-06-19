using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Models
{
    [RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
    public class Cube : MonoBehaviour, IExplodable
    {
        [SerializeField] private float _minLifetime = 2f;
        [SerializeField] private float _maxLifetime = 5f;
        
        private readonly Color _defaultColor = Color.white;
        private readonly Color _collideColor = Color.red;
    
        private bool _isPlatformCollided;
        private MeshRenderer _meshRenderer;
        private Rigidbody _rigidbody;
        private Coroutine _coroutine;

        public event UnityAction<Cube> Released;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnEnable()
        {
            _isPlatformCollided = false;
            SetColor(_defaultColor);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isPlatformCollided || collision.gameObject.TryGetComponent(out Platform _) == false)
            {
                return;
            }

            _isPlatformCollided = true;
            SetColor(_collideColor);
            _coroutine = StartCoroutine(Destroy());
        }
        
        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
    
        private void SetColor(Color color)
        {
            _meshRenderer.material.color = color;
        }
        
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(GetLifetime());
            Released?.Invoke(this);
        }
        
        private float GetLifetime() =>
            Random.Range(_minLifetime, _maxLifetime + 1f);
    }
}