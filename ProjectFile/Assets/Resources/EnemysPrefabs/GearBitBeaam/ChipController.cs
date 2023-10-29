using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ChipController : MonoBehaviour
{
    private List<GameObject> chips = new List<GameObject>();
    public float radius;
    private float centerrad = 0;
    void Start()
    {
        for(int i = 0;i < transform.childCount;i ++){
            chips.Add(transform.GetChild(i).gameObject);
        }
        centerrad = 2 * Mathf.PI /  chips.Count;
    }

    void Update()
    {
        for(int i = 0;i < chips.Count;i ++){
            chips[i].transform.localPosition = new Vector3(0,Mathf.Sin(centerrad*i + Mathf.PI/2)*radius,Mathf.Cos(centerrad*i + Mathf.PI/2)*radius);
        }
    }
    private void OnGUI() {
        for(int i = 0;i < chips.Count;i ++){
            chips[i].transform.localPosition = new Vector3(0,Mathf.Sin(centerrad*i + Mathf.PI/2)*radius,Mathf.Cos(centerrad*i + Mathf.PI/2)*radius);
        }
    }
}
