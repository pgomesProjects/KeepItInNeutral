using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float zRange;
    [SerializeField] private float speed;
    private Vector2 movement;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        rb.AddForce(direction * speed);

        if (transform.position.z < -zRange)
        {
            rb.velocity = Vector3.zero;
        }

        if (transform.position.z > zRange)
        {
            rb.velocity = Vector3.zero;
        }
    }

    //Send value from Move callback to the horizontal Vector2
    public void OnMove(InputAction.CallbackContext ctx) => movement = ctx.ReadValue<Vector2>();
}
