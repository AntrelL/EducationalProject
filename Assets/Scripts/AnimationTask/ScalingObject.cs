using UnityEngine;

public class ScalingObject : TransformableObject
{
    private void FixedUpdate()
    {
        transform.localScale += Vector3.one * Speed;
    }
}
