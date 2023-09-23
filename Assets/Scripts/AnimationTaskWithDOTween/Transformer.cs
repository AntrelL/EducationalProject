using UnityEngine;

public class Transformer : MonoBehaviour
{
    [SerializeField] private float _animationTime;

    protected float AnimationTime 
    { 
        get => _animationTime;
    }
}
