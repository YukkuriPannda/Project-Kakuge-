using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;

public class SetSpecialMagicMotion : MonoBehaviour
{
    [System.Serializable]
    class SpecialMagicMotions{
        public AnimationClip flame;
        public AnimationClip aqua;
        public AnimationClip electro;
        public AnimationClip terra;
    }
    [SerializeField]SpecialMagicMotions specialMagicMotions;
    void Start()
    {
        RuntimeAnimatorController runtimeAnimatorCtl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Models/MainCharactor/PlayerAnimatorBase.controller");
        Debug.Log(runtimeAnimatorCtl.animationClips[0]);
        AnimatorController animController = runtimeAnimatorCtl as UnityEditor.Animations.AnimatorController;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Flame",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.flame;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Aqua",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.aqua;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Electro",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.electro;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Terra",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.terra;
    }
    int GetStateFromName(string name,ChildAnimatorState[] states){
        int result = 0;
        for(; states[result].state.name != name;result ++){
            if(result >= states.Length){
                Debug.LogError("Not Found AnimationState "+name);
                return -1;
            }
        }
        return result;
    }
}
