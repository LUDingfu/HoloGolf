using UnityEngine;
using System.Collections;

public class HoleDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GolfBallController golfBall = other.GetComponent<GolfBallController>();
        if (golfBall != null && golfBall.holeDetectionTrigger)
        {
            Debug.Log("Ball Entered the Hole!");
            StartCoroutine(WaitForBallToStop(other, golfBall));
        }
    }

    private IEnumerator WaitForBallToStop(Collider golfBallCollider, GolfBallController ball)
    {
        while (ball.ActualSpeed > 0.3f)
        {
            yield return new WaitForFixedUpdate();  
        }
        ball.SetBallTrigger(true);
        Debug.Log("Ball has stopped. Now falling into the hole.");
    }
}


