using System;
using System.Collections;
using UnityEngine;

namespace Models
{
    public class Cube : SpawnedObject
    {
        private readonly Color _defaultColor = Color.white;
        private readonly Color _collideColor = Color.red;

        private bool _isPlatformCollided;

        public event Action<Cube> Released;

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
            Coroutine = StartCoroutine(CalculateLifetime());
        }

        private void SetColor(Color color)
        {
            Renderer.material.color = color;
        }

        protected override IEnumerator CalculateLifetime()
        {
            yield return new WaitForSeconds(GetLifetime());
            Released?.Invoke(this);
        }
    }
}