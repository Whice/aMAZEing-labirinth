using System;
using UI;
using UnityEngine;

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
        /// <summary>
        /// Позиция курсора мыши до нажатия на кнопку.
        /// </summary>
        private Vector3 cursorPositionBeforeMouseDown;
        private void OnMouseUp()
        {
            if (this.cursorPositionBeforeMouseDown==Input.mousePosition)
            {
                if (!GameInterfaceRectanglesDetected.instance.isPointerOnUIElement)
                {
                    SimulateOnClick();
                }
            }
        }
        private void OnMouseDown()
        {
            this.cursorPositionBeforeMouseDown = Input.mousePosition;
        }

        #endregion Симуляция/считывание клика по объекту.
    }
}
