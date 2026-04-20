using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemDropAnimator : MonoBehaviour
{
    public void DropTo(Transform target, System.Action onComplete)
    {
        Vector3 start = transform.position;
        Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f)); // 랜덤 퍼짐
        Vector3 end = target.position + offset;

        // 위로 살짝 튀었다가 떨어지는 느낌
        Vector3 mid = (start + end) * 0.5f + Vector3.up * 1.5f;


        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(mid, 0.2f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOMove(end, 0.2f).SetEase(Ease.InQuad));

        seq.OnComplete(() =>
        {
            transform.position = end; // 위치 고정
            onComplete?.Invoke();
        });
    }
}
