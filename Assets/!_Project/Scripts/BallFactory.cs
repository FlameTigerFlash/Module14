using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class BallFactory : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;

    [SerializeField] private Transform _restPoint;

    public Dictionary<GameObject, BallData> BallInfo => _ballInfo;

    private ObjectPool<GameObject> _pool;

    private Dictionary<GameObject, BallData> _ballInfo = new Dictionary<GameObject, BallData>();

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: CreateBall,
            actionOnGet: OnTakeBallFromPool,
            actionOnRelease: OnReturnBall,
            actionOnDestroy: OnDestroyBall,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    private GameObject CreateBall()
    {
        var ball = Instantiate(_ballPrefab);
        var ballData = new BallData(ball);
        _ballInfo[ball] = ballData;

        return ball;
    }

    private void OnTakeBallFromPool(GameObject ball)
    {
        var ballData = new BallData(ball);
        _ballInfo[ball] = ballData;
        ShowBall(ball);
    }

    private void OnReturnBall(GameObject ball)
    {
        HideBall(ball);
    }

    private void OnDestroyBall(GameObject ball)
    {
        HideBall(ball);
        Destroy(ball);
    }

    private void HideBall(GameObject ball)
    {
        if (_ballInfo.ContainsKey(ball))
        {
            _ballInfo[ball].RB.isKinematic = true;
            _ballInfo[ball].RB.linearVelocity = Vector3.zero;
            _ballInfo[ball].RB.angularVelocity = Vector3.zero;

            _ballInfo[ball].TimedDestruction.StopTimer();
            _ballInfo[ball].Controller.enabled = false;
            _ballInfo[ball].Renderer.enabled = false;
            
            foreach (var collider in _ballInfo[ball].Colliders)
            {
                collider.enabled = false;
            }

            _ballInfo.Remove(ball);
        }
        ball.transform.position = _restPoint.position;
    }

    private void ShowBall(GameObject ball)
    {
        _ballInfo[ball].RB.isKinematic = false;

        _ballInfo[ball].Controller.enabled = true;
        _ballInfo[ball].Renderer.enabled = true;

        foreach (var collider in _ballInfo[ball].Colliders)
        {
            collider.enabled = true;
        }
    }

    public GameObject Create(Vector3 position, Quaternion rotation)
    {
        GameObject ball = _pool.Get();
        ball.transform.position = position;
        ball.transform.rotation = rotation;

        return ball;
    }

    public void Delete(GameObject ball)
    {
        _pool.Release(ball);
    }
}

public class BallData
{
    public Rigidbody RB;
    public BallController Controller;
    public TimedDestruction TimedDestruction;
    public Renderer Renderer;
    public Collider[] Colliders;

    public BallData(GameObject ball)
    {
        RB = ball.GetComponent<Rigidbody>();
        Controller = ball.GetComponent<BallController>();
        TimedDestruction = ball.GetComponent<TimedDestruction>();
        Renderer = ball.GetComponent<Renderer>();
        Colliders = ball.GetComponentsInChildren<Collider>();
    }
}
