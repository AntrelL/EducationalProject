using UnityEngine;

public class MovingForwardObject : TransformableObject
{
    private void FixedUpdate()
    {
        Rigidbody.MovePosition(transform.position + transform.forward * Speed);
    }
}
