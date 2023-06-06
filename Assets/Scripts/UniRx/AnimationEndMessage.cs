using SummonEra.RxEvents;

namespace RxEvents
{
    /// <summary>
    /// Событие окончания анимаций.
    /// </summary>
    public class AnimationEndMessage : IRxMsg
    {
        /// <summary>
        /// Анимированный объект.
        /// </summary>
        public object animatedObject;
    }
}