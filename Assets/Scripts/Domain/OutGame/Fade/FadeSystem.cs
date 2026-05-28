using Cysharp.Threading.Tasks;
using UniRx;

namespace Layer.Domain
{
    public class FadeSystem
    {
        public FadeSystem(int fadeTime)
        {
            _fadeTime = fadeTime;
        }

        ReactiveProperty<bool> _isFade = new ReactiveProperty<bool>(false);

        private readonly int _fadeTime;

        public int FadeTime => _fadeTime;

        public async UniTask FadeIn()
        {
            _isFade.Value = false;

            await UniTask.Delay(_fadeTime);
        }

        public async UniTask FadeOut()
        {
            _isFade.Value = true;

            await UniTask.Delay(_fadeTime);
        }
    }
}
