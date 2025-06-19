using System;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class CubeSpawner : Spawner<Cube>
    {
        [SerializeField] private float _repeatRate = 1f;
        [SerializeField] private int _maxPosition = 10;

        public event Action<Cube> Released;

        private void OnEnable()
        {
            InvokeRepeating(nameof(Get), 0f, _repeatRate);
        }

        protected override void OnRelease(Cube cube)
        {
            base.OnRelease(cube);
            cube.Released -= Release;
        }

        protected override void OnGet(Cube cube)
        {
            base.OnGet(cube);
            cube.transform.position = GetRandomPosition();
            cube.Released += Release;
        }

        protected override void Release(Cube cube)
        {
            base.Release(cube);
            Released?.Invoke(cube);
        }

        private Vector3 GetRandomPosition()
        {
            var horizontalPosition = new Vector3(1f, 0f, 1f);

            return horizontalPosition * Random.Range(-_maxPosition, _maxPosition + 1) + transform.position;
        }
    }
}