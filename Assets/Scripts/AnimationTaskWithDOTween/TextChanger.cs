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

        sequence.Append(_text.DOText("��������� �����", animationTimeOfOneOperation));
        sequence.Append(_text.DOText(" � �����������", animationTimeOfOneOperation).SetRelative());
        sequence.Append(_text.DOText("���������� �����", animationTimeOfOneOperation, true, ScrambleMode.All));

        sequence.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
