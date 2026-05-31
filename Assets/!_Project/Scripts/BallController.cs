using UnityEngine;
using UnityEngine.Pool;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _maxSpeed = 10f;

    private ObjectPool<GameObject> _personalPool;

    public float MinY => _minY;
    public float MaxSpeed => _maxSpeed;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _personalPool = pool;
    }

    public void SelfDestruct()
    {
        if (_personalPool != null)
        {
            _personalPool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
