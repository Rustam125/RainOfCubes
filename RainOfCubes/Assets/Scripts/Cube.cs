using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    
    private bool _isPlatformCollided;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    
    private readonly Color _defaultColor = Color.white;
    private readonly Color _collideColor = Color.red;

    public event UnityAction<Cube> Destroyed;

    public void Init(Vector3 position)
    {
        _isPlatformCollided = false;
        _rigidbody.velocity = Vector3.zero;

        transform.rotation = Quaternion.identity;
        transform.position = position;
        gameObject.SetActive(true);

        SetColor(_defaultColor);
    }
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Platform _) ||
            _isPlatformCollided)
        {
            return;
        }

        _isPlatformCollided = true;
        SetColor(_collideColor);
        _coroutine = StartCoroutine(Destroy());
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(GetLifetime());

        gameObject.SetActive(false);
        Destroyed?.Invoke(this);
    }

    private float GetLifetime() => Random.Range(_minLifetime, _maxLifetime + 1f);

    private void SetColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
}