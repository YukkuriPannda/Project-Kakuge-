using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{

    #region [Public parameter]
    public ushort   Health = 65535;
    public bool     Invulnerable = false;
    public bool     NoGravity = false;
    #endregion

    #region [private parameter]
    private bool         OnGround = false;
    //private List<effect> Effects = new List<effect>();
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
