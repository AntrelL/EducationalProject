using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Text))]
public class TextChanger : Transformer
{
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();

        int numberOfOperations = 3;
        float animationTimeOfOneOperation = AnimationTime / numberOfOperations;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_text.DOText("Заменённый текст", animationTimeOfOneOperation));
        sequence.Append(_text.DOText(" с дополнением", animationTimeOfOneOperation).SetRelative());
        sequence.Append(_text.DOText("Взломанный текст", animationTimeOfOneOperation, true, ScrambleMode.All));

        sequence.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
