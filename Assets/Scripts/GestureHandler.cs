using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class GestureHandler : MonoBehaviour
{
    [SerializeField] private GolfBallController ballController;

    private TapGestureRecognizer recognizer = new TapGestureRecognizer();

}