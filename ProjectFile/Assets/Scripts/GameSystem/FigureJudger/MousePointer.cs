using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MousePointer : MonoBehaviour {
    Transform mytrf;
    void Start(){
        mytrf = transform;
    }
    void Update(){
        mytrf.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }   
}