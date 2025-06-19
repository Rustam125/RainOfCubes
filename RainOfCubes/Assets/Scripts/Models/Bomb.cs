using System;
using System.Collections;
using UnityEngine;

namespace Models
{
    public class Bomb : SpawnedObject
    {
        private const float MaxTransparencyValue = 1;
        private const float MinTransparencyValue = 0;

        public event Action<Bomb> Released;

        private void OnEnable()
        {
            Coroutine = StartCoroutine(CalculateLifetime());
        }

        public void Initialize(Vector3 position)
        {
            this.transform.position = position;
            SetTransparency(MaxTransparencyValue);
        }

        private void SetTransparency(float value)
        {
            var color = Renderer.material.color;
            color.a = value;
            Renderer.material.color = color;
        }

        protected override IEnumerator CalculateLifetime()
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
    }
}