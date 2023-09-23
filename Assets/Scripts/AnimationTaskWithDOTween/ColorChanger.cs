using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class ColorChanger : Transformer
{
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;

        _material.color = firstColor;
        _material.DOColor(secondColor, AnimationTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
