using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimedDestruction : MonoBehaviour
{
    [SerializeField] private bool _autoStart = false;

    [SerializeField] private float _initialTime = 10f;

    public UnityEvent TimeOver;

    public float TimeLeft { get; private set; }

    private void Start()
    {
        if (_autoStart)
        {
            SetTimer(_initialTime);
        }
    }

    public void SetTimer(float time)
    {
        StopAllCoroutines();
        StartCoroutine(DeletionCoroutine(time));
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    private IEnumerator DeletionCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        TimeOver.Invoke();
    }
}
