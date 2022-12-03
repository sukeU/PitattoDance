using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnClicked()
    {
        Sequence seq = DOTween.Sequence();
        //(1,1,1)に移動
        seq.Append(
        rectTransform.DOScale(Vector3.one*1.25f, 0.2f)
        );
        //(0,0,0)にスケーリング
        seq.Append(
        rectTransform.DOScale(Vector3.one, 0.4f)
        );
    }
}
