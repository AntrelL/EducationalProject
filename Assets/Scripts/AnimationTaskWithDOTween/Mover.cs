using UnityEngine;
using DG.Tweening;

public class Mover : Transformer
{
    private void Start()
    {
        transform.DOMove(transform.position + new Vector3(5, 0, 0), AnimationTime)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
