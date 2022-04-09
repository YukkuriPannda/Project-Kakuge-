using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigreJudgmenter : MonoBehaviour
{
    private bool tatching = false;
    public Carsol carsol;
    void Start()
    {
        this.transform.position = new Vector2(0,0);
    }
    
    void Update()
    {
        if (carsol.judging && !tatching)
        {
            carsol.candidates.Remove(this.gameObject);
        }
    }
<<<<<<< HEAD
    private void OnCollisionStay2D(Collision2D collision)
=======
    
    private void OnCollisionEnter2D(Collision2D collision)
>>>>>>> f7a584e0b5f0f9e5c6ff2cf24db9d10f052bae44
    {
        tatching = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        tatching = false;
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
