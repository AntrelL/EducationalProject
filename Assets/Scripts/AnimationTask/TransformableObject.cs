using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TransformableObject : MonoBehaviour
{
    [SerializeField] protected float Speed;

    protected Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}
