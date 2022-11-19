using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetPlayer;
    public Vector2 cameraSpeedtoReachTheTarget = new Vector2(0.1f,0.5f);
    void Update()
    {
        transform.Translate((targetPlayer.position - transform.position)* Time.deltaTime / cameraSpeedtoReachTheTarget);
    }
}
