using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] PlayerControl thePlayer;

    [SerializeField] Transform CamPos;
    [SerializeField] Transform CamPosCrouch;

    void Update()
    {
        if(thePlayer.GetCrouched())
        {
            gameObject.transform.position = CamPosCrouch.position;
        }
        else
        {
            gameObject.transform.position = CamPos.position;
        }
    }
}
