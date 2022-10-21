using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardGenerator : MonoBehaviour
{
    [SerializeField] private Texture[] billboardTextures;
    private void OnEnable()
    {
        //Generate a random billboard picture when the object is created
        int randomBillboard = Random.Range(0, billboardTextures.Length);
        GetComponent<Renderer>().material.mainTexture = billboardTextures[randomBillboard];
    }
}
