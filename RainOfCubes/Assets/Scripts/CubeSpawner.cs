using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _maxPostion = 10;
    
    private ObjectPool<Cube> _objectPool;
    
    private void Awake()
    {
        _objectPool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab, transform),
            actionOnGet: GetCube,
            actionOnRelease: ReleaseCube,
            actionOnDestroy: Destroy);
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, _repeatRate);
    }

    private void GetCube(Cube cube)
    {
        cube.Init(GetRandomPosition());
        cube.Destroyed += _objectPool.Release;
    }
    
    private void Spawn()
    {
        _objectPool.Get();
    }
    
    private Vector3 GetRandomPosition()
    {
        var horizontalPosition = new Vector3(1f, 0f, 1f);

        return horizontalPosition * Random.Range(-_maxPostion,
            _maxPostion + 1) + transform.position;
    }

    private void ReleaseCube(Cube cube)
    {
        cube.Destroyed -= _objectPool.Release;
    }
}
