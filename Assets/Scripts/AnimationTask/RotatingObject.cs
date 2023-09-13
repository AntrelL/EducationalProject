using UnityEngine;

public class RotatingObject : TransformableObject
{
    private void FixedUpdate()
    {
        Rigidbody.angularVelocity = Vector3.up * Speed;
    }
}
