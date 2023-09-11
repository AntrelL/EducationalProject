using UnityEngine;

[RequireComponent(typeof(Navigator))]
public class Thief : MovementObject
{
    private Navigator _navigator;

    private void Start()
    {
        _navigator = GetComponent<Navigator>();
    }

    private void FixedUpdate()
    {
        if (Waiting == false)
            MoveTo(_navigator.CurrentTargetPoint.Position);
    }
}
