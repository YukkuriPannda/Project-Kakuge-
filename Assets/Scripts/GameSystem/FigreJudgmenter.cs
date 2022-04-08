using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigreJudgmenter : MonoBehaviour
{
    public bool tatching = false;
    public Carsol carsol;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (carsol.judgmenting && !tatching)
        {
            carsol.candidates.Remove(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        tatching = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        tatching = false;
    }private void FixedUpdate()
    {
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 1; i < this.GetComponent<EdgeCollider2D>().pointCount; i++)
        {
            Gizmos.DrawLine(this.GetComponent<EdgeCollider2D>().points[i-1], this.GetComponent<EdgeCollider2D>().points[i]);
        }
    }
}
