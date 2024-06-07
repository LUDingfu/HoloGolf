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
            StartCoroutine(WaitForBallToStop(other));
        }
    }

    private IEnumerator WaitForBallToStop(Collider golfBallCollider)
    {
        // Wait until the ball's velocity is very close to zero
        while (golfBallCollider.GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
        {
            yield return new WaitForFixedUpdate();  // Wait for the next physics update
        }
    
        golfBallCollider.GetComponent<Collider>().isTrigger = true;
        Debug.Log("Ball has stopped. Now falling into the hole.");
    }
}


