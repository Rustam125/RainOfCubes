using Models;
using UnityEngine;

namespace Spawners
{
    public class BombSpawner : Spawner<Bomb>
    {
        [SerializeField] private CubeSpawner _cubeSpawner;
        [SerializeField] private Exploder _exploder;

        private Vector3 _position;

        private void OnEnable()
        {
            _cubeSpawner.Released += GetBomb;
        }

        private void OnDisable()
        {
            _cubeSpawner.Released -= GetBomb;
        }

        protected override void OnGet(Bomb bomb)
        {
            base.OnGet(bomb);
            bomb.Initialize(_position);
            bomb.Released += Release;
        }

        protected override void OnRelease(Bomb bomb)
        {
            base.OnRelease(bomb);
            bomb.Released -= Release;
        }

        protected override void Release(Bomb bomb)
        {
            _exploder.Explode(bomb);
            base.Release(bomb);
        }

        private void GetBomb(Cube cube)
        {
            _position = cube.transform.position;
            Get();
        }
    }
}