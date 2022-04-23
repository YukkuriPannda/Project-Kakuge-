using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField] ContactFilter2D filter2d;

    #region [Public Parameters]
    public ushort   Health = 65535;
    public bool     Invulnerable = false;
    public bool     NoGravity = false;
    #endregion

    #region [Private Parameters]
    public bool     OnGround = false;
    private Rigidbody2D rg2D;
    //private List<effect> Effects = new List<effect>();
    #endregion

    void Start ()
    {
        rg2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

	void Update ()
    {
        OnGround = rg2D.IsTouching(filter2d);
	}
}
