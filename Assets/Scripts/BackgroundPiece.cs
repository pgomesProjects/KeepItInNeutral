using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPiece : MonoBehaviour
{
    private float destroyX = -30;
    private float backgroundSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.IsGameActive())
        {
            transform.position -= new Vector3(backgroundSpeed, 0, 0) * Time.deltaTime;

            if (transform.localPosition.x <= destroyX)
                Destroy(gameObject);
        }
    }

    public void SetMoveSpeed(float speed)
    {
        backgroundSpeed = speed;
    }
}
