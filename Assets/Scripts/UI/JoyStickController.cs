using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JoyStickController : MonoBehaviour
{
    public Image joystick_Body;
    public float radius = 50;

    [Space (10)]
    [Header("Output")]
    public Vector2 joystickInfo;
    LineRenderer lineRenderer;
    bool tatchingJoystick = false;
    /*
    this script is joystick controller.
    joystickInfo output
      ↑ y > 0
      ↓ y < 0
      → x > 0
      ← x < 0
    */
    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }
   
    void Update()
    {
        if (JoystickDistance() <= radius && Input.GetKeyDown(KeyCode.Mouse0)) tatchingJoystick = true;
        if (Input.GetKeyUp(KeyCode.Mouse0)) tatchingJoystick = false;

        if (tatchingJoystick)
        {
            joystickInfo = JoystickInfoOutPut() / radius;

            joystick_Body.rectTransform.localPosition = new Vector3(JoystickInfoOutPut().x, JoystickInfoOutPut().y, 0);
        }
        else
        {
            joystickInfo = new Vector2(0, 0); 
            joystick_Body.rectTransform.localPosition = new Vector3(joystickInfo.x, joystickInfo.y, 0);
        }

        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(joystick_Body.transform.position));
        lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(this.transform.position));
    }
    
    Vector2 JoystickInfoOutPut()
    {
        if (JoystickDistance() <= radius)
        {
            return new Vector2(Input.mousePosition.x - this.transform.position.x, Input.mousePosition.y - this.transform.position.y);
        }
        else
        {
            return new Vector2(radius * (Input.mousePosition.x - this.transform.position.x) / JoystickDistance(), radius * (Input.mousePosition.y - this.transform.position.y) / JoystickDistance());
        }
    }
    float JoystickDistance()
    {
        return Mathf.Sqrt((Input.mousePosition.x - this.transform.position.x) * (Input.mousePosition.x - this.transform.position.x)
            + (Input.mousePosition.y - this.transform.position.y) * (Input.mousePosition.y - this.transform.position.y));
    }
}
