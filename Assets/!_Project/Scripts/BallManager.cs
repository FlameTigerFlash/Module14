using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using System;

public class BallManager : MonoBehaviour
{
    [SerializeField] private BallFactory _ballFactory;

    private void Update()
    {
        HandlePositions();
    }

    private void HandlePositions()
    {
        var ballInfo = _ballFactory.BallInfo;
        List<GameObject> toDelete = new List<GameObject>();

        foreach (GameObject ball in ballInfo.Keys)
        {
            var rb = ballInfo[ball].RB;
            var controller = ballInfo[ball].Controller;

            if (controller.MinY > rb.position.y && controller.MaxSpeed < Vector3.Magnitude(rb.linearVelocity))
            {
                toDelete.Add(ball);
            }
        }

        foreach (GameObject ball in toDelete)
        {
            _ballFactory.Delete(ball);
        }
    }
}
