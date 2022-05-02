using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Скрипт для перемещения камеры по игровому полю.
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        /// <summary>
        /// Данные о главной камере.
        /// </summary>
        public new Transform camera;
        /// <summary>
        /// Скорость перемещения камеры.
        /// </summary>
        [SerializeField]
        private Single moveSpeed = 0.05f;

        /// <summary>
        /// Прямоугольник захвата курсора с правой стороны.
        /// </summary>
        private Rect rightSide;
        /// <summary>
        /// Данные об области захвата курсора с правой стороны. 
        /// </summary>
        [SerializeField]
        private RectTransform rightSideRectTransform;
        /// <summary>
        /// Прямоугольник захвата курсора с левой стороны.
        /// </summary>
        private Rect leftSide;
        /// <summary>
        /// Данные об области захвата курсора с левой стороны. 
        /// </summary>
        [SerializeField]
        private RectTransform leftSideRectTransform;
        /// <summary>
        /// Прямоугольник захвата курсора с верхней стороны.
        /// </summary>
        private Rect topSide;
        /// <summary>
        /// Данные об области захвата курсора с верхней стороны. 
        /// </summary>
        [SerializeField]
        private RectTransform topSideRectTransform;
        /// <summary>
        /// Прямоугольник захвата курсора с нижней стороны.
        /// </summary>
        private Rect bottomSide;
        /// <summary>
        /// Данные об области захвата курсора с нижней стороны. 
        /// </summary>
        [SerializeField]
        private RectTransform bottomSideRectTransform;

        /// <summary>
        /// Преобразовать данные об области захвата в прямоугольник захвата.
        /// </summary>
        /// <param name="sideRectTransform"></param>
        /// <returns></returns>
        private Rect CalculateRectFromRectTransform(RectTransform sideRectTransform)
        {
            return new Rect(
                new Vector2
                (
                    sideRectTransform.position.x - sideRectTransform.rect.size.x / 2,
                    sideRectTransform.position.y - sideRectTransform.rect.size.y / 2
                    ),
                sideRectTransform.rect.size);
        }



        private void Awake()
        {
            this.rightSide = CalculateRectFromRectTransform(this.rightSideRectTransform);
            this.leftSide = CalculateRectFromRectTransform(this.leftSideRectTransform);
            this.topSide = CalculateRectFromRectTransform(this.topSideRectTransform);
            this.bottomSide = CalculateRectFromRectTransform(this.bottomSideRectTransform);
        }
        private void Update()
        {
            if (this.rightSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x + this.moveSpeed,
                    this.camera.position.y,
                    this.camera.position.z
                    );
            }
            else if (this.leftSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x - this.moveSpeed,
                    this.camera.position.y,
                    this.camera.position.z
                    );
            }
            if (this.topSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x,
                    this.camera.position.y,
                    this.camera.position.z + this.moveSpeed
                    );
            }
            else if (bottomSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x,
                    this.camera.position.y,
                    this.camera.position.z - this.moveSpeed
                    );
            }
        }
    }
}
