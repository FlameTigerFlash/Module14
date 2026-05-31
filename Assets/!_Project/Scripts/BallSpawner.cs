using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private BallFactory _ballFactory;

    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _spawnPeriod = 0.2f;
    [SerializeField] private float _randomSphereRadius = 0.5f;
    [SerializeField] private float _ballLifetime = 20f;

    [SerializeField] private bool _autoStart = true;

    private void Start()
    {
        if (_autoStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnCoroutine(_spawnPeriod));
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    private void SpawnBall()
    {
        Vector3 offset = Random.insideUnitSphere * _randomSphereRadius;
        var ball = _ballFactory.Create(_spawnPoint.position + offset, _spawnPoint.rotation);
        var timedDestruction = ball.GetComponent<TimedDestruction>();
        timedDestruction.SetTimer(_ballLifetime);
    }

    private IEnumerator SpawnCoroutine(float period)
    {
        while (true)
        {
            yield return new WaitForSeconds(period);
            SpawnBall();
        }
    }
}
