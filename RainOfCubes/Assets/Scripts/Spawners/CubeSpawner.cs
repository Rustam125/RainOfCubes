using System;
using System.Collections;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public sealed class CubeSpawner : Spawner<Cube>
    {
        [SerializeField] private float _repeatRate = 1f;
        [SerializeField] private int _maxPosition = 10;

        private Coroutine _coroutine;
        private WaitForSeconds _delay;
        
        public event Action<Cube> Released;

        protected override void Awake()
        {
            base.Awake();
            _delay = new WaitForSeconds(_repeatRate);
        }
        
        private void OnEnable()
        {
            _coroutine = StartCoroutine(GetCubesOverTime());
        }

        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
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

        private IEnumerator GetCubesOverTime()
        {
            while (gameObject.activeSelf)
            {
                yield return _delay;
                Get();
            }
        }
    }
}