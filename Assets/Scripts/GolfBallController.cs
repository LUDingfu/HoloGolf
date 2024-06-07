using UnityEngine;
using Random = UnityEngine.Random;

public class GolfBallController : MonoBehaviour
{
    [SerializeField] private float speedFactor = 10.0f; 
    [SerializeField] private Transform holeTransform;
    
    [Header("Probabilities")]
    [Tooltip("Chance of ball being sunk into the hole.")]
    [Range(0,100)][SerializeField] private int correctPercentage;
    [Tooltip("If miss, chance of ball to have a wrong angle. " +
             "The lower this chance, the more likely it will overshoot or undershoot")]
    [Range(0,100)] [SerializeField] private int wrongAngleChance;
    [Tooltip("Ratio of the ball to undershoot or overshoot. " +
             "If it misses, but the angle is correct, " +
             "this determines if it overshoot or undershoots")]
    [Range(0,100)] [SerializeField] private int undershootToOvershootRatio;
    
    [Header("Parameters")] 
    [SerializeField] private float undershootDistance; //it cannot greater than the distance between golf and hole
    [SerializeField] private float overshootDistance;
    [SerializeField] private int wrongAngleDistance;
    [SerializeField] private ShotResult currentShotResult;
    
    private new Rigidbody rigidbody;
    private bool hasFired;
    private bool directionFromGolfToHoleLock;
    private Vector3 targetDirectionNormalized;
    private bool wrongAngleLock;
    private Vector3 wrongAngleVector;
    private Vector3 wrongAngleVectorNormalised;

    private enum ShotResult { IntoHole, WrongAngle, Undershoot, Overshoot }

    
    public bool holeDetectionTrigger;
    
    void Start()
    {
        targetDirectionNormalized = Vector3.zero;
        wrongAngleVector = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        ComputeShotType();
        Debug.Log(currentShotResult);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            hasFired = true;
        }
        FireBall();
    }

    private void FireBall()
    {
        MoveBall();
    }

    private void ComputeShotType()
    {
        int roll = Random.Range(0, 100);
        if (roll < correctPercentage) currentShotResult = ShotResult.IntoHole;
        else
        {
            roll = Random.Range(0, 100);
            if (roll < wrongAngleChance)
            {
                currentShotResult = ShotResult.WrongAngle;
            }
            else
            {
                roll = Random.Range(0, 100);
                currentShotResult = roll < undershootToOvershootRatio ? ShotResult.Overshoot : ShotResult.Undershoot;
            }
        }
    }


    private void MoveBall()
    {
        if (!hasFired) return;
        
        Vector3 targetDirectionVector = (holeTransform.position - this.transform.position);
        float targetDirectionVectorMagnitude = targetDirectionVector.magnitude;
        if (!directionFromGolfToHoleLock)
        {
            targetDirectionNormalized = targetDirectionVector.normalized;
            directionFromGolfToHoleLock = true;
        }
        
        if (!wrongAngleLock)
        { 
            Vector3 randomAngle = GiveWrongAngle();
            wrongAngleVector = holeTransform.position + randomAngle * wrongAngleDistance;
            wrongAngleVectorNormalised = (wrongAngleVector - transform.position).normalized;
            wrongAngleLock = true;
        }

        switch (currentShotResult)
        {
            case ShotResult.IntoHole:
                holeDetectionTrigger = true;
                SetActualSpeed(targetDirectionNormalized, targetDirectionVectorMagnitude, speedFactor);
                if (rigidbody.velocity.magnitude < 0.9f) speedFactor = 0;
                break;
            
            case ShotResult.WrongAngle:
                float targetSpeed = (wrongAngleVector - transform.position).magnitude;
                SetActualSpeed(wrongAngleVectorNormalised, targetSpeed, speedFactor);
                break;
            
            case ShotResult.Undershoot:
                float actualDistanceUndershoot = targetDirectionVectorMagnitude - undershootDistance;
                SetActualSpeed(targetDirectionNormalized,actualDistanceUndershoot,speedFactor);
                break;
            
            case ShotResult.Overshoot:
                float actualDistanceOvershoot = (targetDirectionVector + overshootDistance * targetDirectionNormalized).magnitude;
                SetActualSpeed(targetDirectionNormalized,actualDistanceOvershoot,speedFactor);
                break;
            
        }
        rigidbody.velocity += new Vector3(0,-10,0);
    }

    private Vector3 GiveWrongAngle()
    {
        float xCheck = -1;
        Vector3 direction = Vector3.zero;
        while (xCheck < 0.5f)
        {
            direction = Random.insideUnitSphere;
            xCheck = direction.x;
        }
        direction = new Vector3(direction.x, 0f, direction.z).normalized;
        return direction;
    }

    private void SetActualSpeed(Vector3 targetDirectionNormalized, float directionVectorMagnitude, float speedFactor)
    {
        float actuallySpeed = speedFactor * directionVectorMagnitude;
        rigidbody.velocity = targetDirectionNormalized * actuallySpeed;
    }
}