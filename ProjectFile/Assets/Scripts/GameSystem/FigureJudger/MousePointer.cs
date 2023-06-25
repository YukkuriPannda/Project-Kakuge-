using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MousePointer : MonoBehaviour {
    Transform mytrf;
    Camera maincamera;
    public bool inGameWindow;
    void Start(){
        mytrf = transform;
        maincamera = Camera.main;
    }
    void Update(){
        mytrf.position = (Vector2)maincamera.ScreenToWorldPoint(Input.mousePosition);
        inGameWindow = (Input.mousePosition.x < maincamera.pixelWidth || Input.mousePosition.y < maincamera.pixelHeight
        || Input.mousePosition.x > 0 || Input.mousePosition.y > 0);
    }   
}
