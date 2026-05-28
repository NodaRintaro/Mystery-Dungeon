using UnityEngine;
using DG.Tweening;

namespace Layer.View
{
    /// <summary> 往復Animationを行うUI </summary>
    public class OscillationAnimationUI : MonoBehaviour
    {
        [Header("周期")]
        [SerializeField, Tooltip("周期")] private float _period = 1f;

        [Header("往復方向")]
        [SerializeField, Tooltip("往復方向")] private Vector2 _direction = Vector2.up;

        [Header("ループ回数")]
        [SerializeField,Tooltip("Loops")] private int _loops = -1;

        // RectTransformをキャッシュする変数
        private RectTransform _rectTransform = null;

        // 初期位置を保存する変数
        private Vector3 _startPos;

        private void Awake()
        {
            if(TryGetComponent(out RectTransform rectTransform))
            {
                _rectTransform = rectTransform;
                _startPos = _rectTransform.anchoredPosition;
            }
        }

        private void OnEnable()
        {
            if(_rectTransform == null) return;

            // アンカーポジションを振幅分移動させるアニメーション
            _rectTransform.DOAnchorPos(_direction + _rectTransform.anchoredPosition, _period).SetLoops(_loops, LoopType.Yoyo).SetLink(gameObject);
        }

        private void OnDisable()
        {
            _rectTransform.anchoredPosition = _startPos;
            transform.DOKill();
        }
    }
}
