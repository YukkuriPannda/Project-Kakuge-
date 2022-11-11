using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour
{
    #region [Public Parameters]
    public RectTransform joystick_Body;
    public float radius = 50f;//半径
    public float deadzone = 0.2f; //半径に対するデッドゾーン割合(百分率)
    private float JumpTimeLimit = 0.4f;//ジャンプを認識してからのリミット(s)
    [SerializeField] PlayerController playerController;
    [SerializeField] Text debconsole;
    [SerializeField] bool IsJsTouching = false;
    #endregion

    #region [Private Parameters]
    private RectTransform rectTransform;
    private int jsFingerId = -1;
    private float JumpForceTime = 0f;
    #endregion

    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 InputPosition;

        var touchCount = Input.touchCount;
        for (int i = 0; i < touchCount; i++) //
        {
            var touch = Input.GetTouch(i);
            if (touch.position.x < Screen.width/2) //画面半分で描画と操作を分ける(仮)
            {
                if (!IsJsTouching) jsFingerId = touch.fingerId;
            } else {
                //ShapeRecognizerへ
            }

            if (touch.fingerId == jsFingerId) {
                switch (touch.phase) {
                    case TouchPhase.Began:
                        rectTransform.position = touch.position;
                        IsJsTouching = true;
                        goto case TouchPhase.Moved;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        InputPosition = touch.position;

                        Func<float> JoystickDistance = () => Vector2.Distance(InputPosition , new Vector2(rectTransform.position.x , rectTransform.position.y));
                        Vector2 JsOutPos = new Func<Vector2>(() => { //デッドゾーン、最大値制限
                            if (JoystickDistance() >= radius)
                            {
                                return new Vector2(
                                    radius * (InputPosition.x - rectTransform.position.x) / JoystickDistance(),
                                    radius * (InputPosition.y - rectTransform.position.y) / JoystickDistance()
                                );
                            } else if (JoystickDistance() <= radius*deadzone) {
                                return Vector2.zero;
                            } else {
                                return new Vector2(
                                    InputPosition.x - rectTransform.position.x,
                                    InputPosition.y - rectTransform.position.y
                                );
                            }
                        })();
                        joystick_Body.localPosition = JsOutPos;
                        Vector2 JoystickOutPut = JsOutPos / radius; //最大値を1とする実際のインプットになる
                        debconsole.text = "" + JumpForceTime;
                        playerController.Move(JoystickOutPut.x);
                        if (JoystickOutPut.y >= 0.4f && JumpForceTime < JumpTimeLimit) { //y軸4割り以上のとき、JumpTimeLimit秒間にわたって上向きの力を与える
                            JoystickOutPut.y *= 1.11f; if (JoystickOutPut.y > 1f) JoystickOutPut.y = 1f; //入力を1.11倍、最大値を1.0にする(0.90以上の入力は最大値と見做す)
                            playerController.Jump((JoystickOutPut.y/JumpTimeLimit)*Time.deltaTime); //JumpTimeLimit秒間で合計1になるように(理論値)
                            JumpForceTime += Time.deltaTime;
                            if (JumpForceTime >= JumpTimeLimit) playerController.Jump(JoystickOutPut.y*Time.deltaTime);
                        } else if (JoystickOutPut.y <= 0.4f || JumpForceTime >= JumpTimeLimit) JumpForceTime = 0f;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        joystick_Body.localPosition = Vector3.zero;
                        jsFingerId = -1;
                        IsJsTouching = false;
                        break;
                }
            }

        }
    }

}