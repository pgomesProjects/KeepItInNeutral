using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScroll : MonoBehaviour
{
    private Renderer textureRend2;
    private Material grassMat;
    private float scrollSpeed;

    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        textureRend2 = GetComponent<Renderer>();
        grassMat = textureRend2.material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        grassMat.mainTextureOffset = new Vector2(-offset, 0);
    }

    public void UpdateScrollSpeed(float speed)
    {
        scrollSpeed = speed;
    }
}
