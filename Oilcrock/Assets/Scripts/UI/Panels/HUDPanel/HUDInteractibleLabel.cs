using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Player.Modules.Interactible;

namespace UI.Panels.HUD
{
    public class HUDInteractibleLabel : UIElement
    {
        [SerializeField] private Transform _backgroundTransform;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _textButton;
        [SerializeField] private TextMeshProUGUI _textName;

        [Space]
        [Header("Tweens settings")]
        [SerializeField] private Ease _backgroundEase;
        [SerializeField, Range(-500, 0)] private float _backgroundMove = -100;
        [SerializeField] private float _backgroundAlfa = 70;
        [SerializeField] private float _backgroundEaseTime;
        [Space]
        [SerializeField] private Ease _textEase;
        [SerializeField] private float _textEaseTime;

        private Tween _backgroundMoveTween;
        private Tween _backgroundColorTween;
        private Tween _textButtonTween;
        private Tween _textNameTween;


        private void OnValidate()
        {
            _backgroundTransform ??= transform;

            _backgroundImage ??= GetComponentInChildren<Image>();

            if (_textButton == null || _textName == null)
                Debug.LogError("Not set a TextLabel to the HUDInteractionLabel!");
        }

        private void Awake()
        {
            // Hide without animation
            base.Hide();
        }

        private void TextTweenFade(Tween tween, TextMeshProUGUI text, bool isFade)
        {
            tween?.Kill();
            tween = text.DOFade(isFade ? 0 : 1, _textEaseTime)
                .From(isFade ? 1 : 0).SetDelay(isFade ? 0 : _backgroundEaseTime - _textEaseTime)
                .SetEase(_backgroundEase);
        }


        [ContextMenu("UI/Hide")]
        public override void Hide()
        {
            TextTweenFade(_textButtonTween, _textButton, true);
            TextTweenFade(_textNameTween, _textName, true);

            _backgroundMoveTween?.Kill();
            _backgroundColorTween?.Kill();

            _backgroundMoveTween =
                _backgroundTransform.DOLocalMoveY(_backgroundMove, _backgroundEaseTime)
                .From(0).SetEase(_textEase);

            _backgroundColorTween =
                _backgroundImage.DOFade(0, _backgroundEaseTime) // base.Hide() OnComplete!
                .From(_backgroundAlfa).SetEase(_backgroundEase).OnComplete(base.Hide);
        }

        [ContextMenu("UI/Show")]
        public override void Show()
        {
            //_backgroundTransform.Translate(Vector3.down * _backgroundMove);


            TextTweenFade(_textButtonTween, _textButton, false);
            TextTweenFade(_textNameTween, _textName, false);

            _backgroundMoveTween?.Kill();
            _backgroundColorTween?.Kill();

            _backgroundMoveTween =
                _backgroundTransform.DOLocalMoveY(0, _backgroundEaseTime)
                .From(_backgroundMove).SetEase(_backgroundEase);

            _backgroundColorTween =
                _backgroundImage.DOFade(_backgroundAlfa, _backgroundEaseTime)
                .From(0).SetEase(_backgroundEase);

            base.Show();
        }

        public void ShowInteract(IInteractable interactible)
        {
            _textName.text = interactible.InteractName;

            Show();
        }
    }
}