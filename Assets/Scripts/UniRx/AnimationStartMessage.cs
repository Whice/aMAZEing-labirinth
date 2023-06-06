using SummonEra.RxEvents;

namespace RxEvents
{
    /// <summary>
    /// Событие начала анимаций.
    /// </summary>
    public class AnimationStartMessage : IRxMsg
    {
        /// <summary>
        /// Анимированный объект.
        /// </summary>
        public object animatedObject;
    }
}