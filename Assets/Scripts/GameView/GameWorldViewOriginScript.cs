using System;
using UI;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Класс для объектов находящихся в простанстве сцены, на не UI.
    /// </summary>
    public abstract class GameWorldViewOriginScript: GameViewOriginScript
    {
        #region Симуляция/считывание клика по объекту.

        /// <summary>
        /// Событие клика на стрелочку.
        /// </summary>
        public event Action OnSlotClicked;
        /// <summary>
        /// Симулировать клик по объекту.
        /// </summary>
        public virtual void SimulateOnClick()
        {
            this.OnSlotClicked?.Invoke();
        }
        private void OnMouseUp()
        {
            if (!GameInterfaceRectanglesDetected.instance.isPointerOnUIElement)
            {
                SimulateOnClick();
            }
        }
        private void OnMouseDown()
        {
        }

        #endregion Симуляция/считывание клика по объекту.
    }
}
