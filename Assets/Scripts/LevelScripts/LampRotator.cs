using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampRotator : MonoBehaviour
{
    private void OnEnable()
    {
        //Rotate the lamp if on the right side of the road
        if(transform.localPosition.z < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
