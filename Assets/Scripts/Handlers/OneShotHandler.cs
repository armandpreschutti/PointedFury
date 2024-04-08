using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotHandler : MonoBehaviour
{
    public float DestructionTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, DestructionTime);
    }
}
