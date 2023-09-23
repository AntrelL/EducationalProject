using UnityEngine;
using DG.Tweening;

public class Rotator : Transformer
{
    private void Start()
    {
        transform.DOBlendableRotateBy(new Vector3(0, 360, 0), AnimationTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
