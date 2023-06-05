using System;
using System.Linq;
using UniRx;
using DG.Tweening;

namespace SummonEra.RxEvents
{
    public static class RxEventExtension
    {
        public static CompositeDisposable Subscribe<TEvent>(this CompositeDisposable disposables, Action<TEvent> msg) where TEvent : IRxMsg
        {
            MessageBroker.Default
                .Receive<IRxMsg>()
                .Where(x => x.GetType() == typeof(TEvent))
                .Select(x => (TEvent)x)
                .Subscribe(msg)
                .AddTo(disposables);
            return disposables;
        }

        public static void Publish(this IRxMsg msg)
        {
            MessageBroker.Default.Publish(msg);
        }

        public static void Publish(this IRxMsg msg, float delay)
        {
            float t = 0f;
            DOTween.To(() => t, x => t = x, delay, delay).OnComplete(() =>
            {
                msg.Publish();
            });
        }
    }
}
