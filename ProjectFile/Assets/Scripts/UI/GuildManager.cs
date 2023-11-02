using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public PlayerController plc;
    public List<Request> requests;
    public class Request{
        public string title;
        public List<Goal> goals = new List<Goal>();
        public NPCController requester;
        
        public class Goal{
            public string goalTitle;
            public KeyCode keyCode = KeyCode.None;
            public string drawShapeName = "None";
            public Goal(string goalTitle,KeyCode keyCode){
                this.goalTitle = goalTitle;
                this.keyCode = keyCode;
            }
            public Goal(string goalTitle,string drawShapeName){
                this.goalTitle = goalTitle;
                this.drawShapeName = drawShapeName;
            }
        }
        public Request(string title,Goal[] goals,NPCController requester){
            this.title = title;
            this.goals = new List<Goal>(goals);
            this.requester = requester;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(requests[0].goals.Count > 0)if((requests[0].goals[0].keyCode != KeyCode.None &&Input.GetKeyDown(requests[0].goals[0].keyCode))
        || (requests[0].goals[0].drawShapeName != "None" &&requests[0].goals[0].drawShapeName == plc.drawShapeName)){
            requests[0].goals.RemoveAt(0);
            if(requests[0].goals.Count <= 0){
                requests.RemoveAt(0);
            }
        }
    }
    public void MakeRequest(Request request){
        requests.Add(request);
    }
}
