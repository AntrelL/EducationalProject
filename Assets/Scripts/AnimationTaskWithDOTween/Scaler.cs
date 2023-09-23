using UnityEngine;
using DG.Tweening;

public class Scaler : Transformer
{
    private void Start()
    {
        transform.DOScale(Vector3.one * 2f, AnimationTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
