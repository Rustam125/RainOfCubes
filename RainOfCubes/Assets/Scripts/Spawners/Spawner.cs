using System;
using Models;
using UnityEngine;
using UnityEngine.Pool;

namespace Spawners
{
    public class Spawner<T> : MonoBehaviour where T : SpawnedObject
    {
        [SerializeField] private T _prefab;
        [SerializeField] private int _poolCapacity = 5;
        [SerializeField] private int _poolMaxSize = 5;

        private ObjectPool<T> _pool;
        private SpawnerInfo _spawnerInfo;

        public event Action<SpawnerInfo> ChangedSpawnerInfo;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<T>(
                createFunc: Create,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: @object => Destroy(@object.gameObject),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaxSize
            );

            _spawnerInfo = new SpawnerInfo(typeof(T).Name, 0, 0, 0);
            ChangedSpawnerInfo?.Invoke(_spawnerInfo);
        }

        protected virtual void OnRelease(T @object)
        {
            @object.gameObject.SetActive(false);

            _spawnerInfo.SetCountActiveObjects(_pool.CountActive);
            ChangedSpawnerInfo?.Invoke(_spawnerInfo);
        }

        protected virtual void OnGet(T @object)
        {
            @object.gameObject.SetActive(true);

            _spawnerInfo.IncreaseSpawnedObjects();
            _spawnerInfo.SetCountActiveObjects(_pool.CountActive);
            ChangedSpawnerInfo?.Invoke(_spawnerInfo);
        }

        protected void Get()
        {
            _pool.Get();
        }

        protected virtual void Release(T @object)
        {
            _pool.Release(@object);
        }
        
        private T Create()
        {
            _spawnerInfo.IncreaseCreatedObjects();
            ChangedSpawnerInfo?.Invoke(_spawnerInfo);

            return Instantiate(_prefab);
        }
    }
}