using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnrealTeam.SB.Services.Other;

namespace UnrealTeam.SB.Services.Loading
{
    public class LoadingCurtain
    {
        private readonly ObjectsProvider _objectsProvider;
        private CancellationTokenSource _tokenSource;


        public LoadingCurtain(ObjectsProvider objectsProvider)
        {
            _objectsProvider = objectsProvider;
        }

        public async UniTask ShowAsync()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            var transitionAnimator = _objectsProvider.CurtainRefs.TransitionAnimator;
            float duration = transitionAnimator.profile.duration;
            float timer = transitionAnimator.progress * duration;
            
            await UniTask.WaitUntil(() =>
            {
                if (timer >= duration)
                {
                    transitionAnimator.SetProgress(1);
                    return true;
                }
                
                transitionAnimator.SetProgress(timer/duration);
                timer += Time.deltaTime;
                return false;
            }, cancellationToken: token);
        }

        public async UniTask HideAsync()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            
            var transitionAnimator = _objectsProvider.CurtainRefs.TransitionAnimator;
            float duration = transitionAnimator.profile.duration;
            float timer = transitionAnimator.progress * duration;
            
            await UniTask.WaitUntil(() =>
            {
                if (timer < 0)
                {
                    transitionAnimator.SetProgress(0);
                    return true;
                }
                
                transitionAnimator.SetProgress(timer/duration);
                timer -= Time.deltaTime;
                return false;
            }, cancellationToken: token);
        }
    }
}