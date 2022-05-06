using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// Скрипт для перемещения камеры по игровому полю.
    /// </summary>
    public class MoveCamera : MonoBehaviour, IDragHandler
    {
        #region Данные камеры.

        /// <summary>
        /// Данные о главной камере.
        /// </summary>
        public new Transform camera;

        #region Скорость движения камеры.

        //Скорость расчитывается для разрешения 1920х1080,
        //потому нужно масштабирование скорости для других разрешений.
        //Предполагается, что экран всегда лежит горизонтально.

        /// <summary>
        /// Исходное разрешение, для которого расчитываются все скорости.
        /// </summary>
        private const Single  ORIGIN_RESOLUTION_WIDTH = 1920f;
        /// <summary>
        /// Прямоугльник главного канваса. Нужен, чтобы знать текущее разрешение.
        /// </summary>
        [SerializeField]
        private RectTransform rectForResolution;
        /// <summary>
        /// Координаты центра экрана.
        /// </summary>
        private Vector2 centerCoordinate;
        /// <summary>
        /// Расчитать значение скорости для нестандартного разрешения (не 1920х1080).
        /// </summary>
        private Single resolutionScaling
        {
            get
            {
                Single largeSide = this.rectForResolution.rect.width > this.rectForResolution.rect.height ? this.rectForResolution.rect.width : this.rectForResolution.rect.height;
                return ORIGIN_RESOLUTION_WIDTH / largeSide;
            }
        }


        /// <summary>
        /// Скорость перемещения камеры.
        /// </summary>
        [SerializeField]
        private Single moveSpeed = 0.03f;
        /// <summary>
        /// Увеличенная скорость перемещения камеры. Для движения правой кнопкой мыши.
        /// </summary>
        private Single increasedMoveSpeed = 99;
        /// <summary>
        /// Множитель при нажатии правой кнопки мыши.
        /// </summary>
        public Int32 rightClickMultiplier;
        /// <summary>
        /// Правая кнопка мыши зажата.
        /// </summary>
        private Boolean isRightButtonPressed = false;
        /// <summary>
        /// Предыдущая позиция мыши.
        /// </summary>
        private Vector2 firsrtMousePositionWithRightDown = Vector2.zero;
        /// <summary>
        /// Разница между первым и нынешним кадром при зажатой правой клавише мыши.
        /// </summary>
        private Vector2 deltaMousePositionWithRightDown
        {
            get => this.firsrtMousePositionWithRightDown - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        /// <summary>
        /// Удаленность курсора от центра в процентном соотношении, 
        /// где центр экрана (0, 0), а левый край по центру, к примеру, (-1,0).
        /// </summary>
        private Vector2 distanceOfCursorFromCenterInPercent
        {
            get => new Vector2
                (
                (Input.mousePosition.x-this.centerCoordinate.x)/this.centerCoordinate.x,
                (Input.mousePosition.y-this.centerCoordinate.y)/this.centerCoordinate.y
                );
        }


        /// <summary>
        /// Множитель для скорости перемещения камеры при касании на андроиде.
        /// </summary>
        private Single androidSpeedMultiplier = 0.15f;

        #endregion Скорость движения камеры.

        #endregion Данные камеры.

        #region Прямоугольники для отлавливания касания края экрана.

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



        #endregion Прямоугольники для отлавливания касания края экрана.

        #region Функции передвижения камеры.

        /// <summary>
        /// Передвинуть камеру над игровым полем.
        /// </summary>
        /// <param name="xShift">Смещение вправо/влево.</param>
        /// <param name="zShift">Смещение вниз/вверх.</param>
        private void Move(Single xShift, Single zShift)
        {

            this.camera.position = new Vector3
                    (
                    this.camera.position.x + xShift * this.moveSpeed,
                    this.camera.position.y,
                    this.camera.position.z + zShift * this.moveSpeed
                    );
        }
        /// <summary>
        /// Передвинуть камеру над игровым полем вправо.
        /// </summary>
        private void MoveRight()
        {
            Move(1, 0);
        }
        /// <summary>
        /// Передвинуть камеру над игровым полем влево.
        /// </summary>
        private void MoveLeft()
        {
            Move(-1, 0);
        }
        /// <summary>
        /// Передвинуть камеру над игровым полем вверх.
        /// </summary>
        private void MoveUp()
        {
            Move(0, 1);
        }
        /// <summary>
        /// Передвинуть камеру над игровым полем вниз.
        /// </summary>
        private void MoveDown()
        {
            Move(0, -1);
        }

        #endregion Функции передвижения камеры.

        private void Awake()
        {
            //Расчитать значение скорости для нестандартного разрешения (не 1920х1080)
            this.moveSpeed *= this.resolutionScaling;
            this.increasedMoveSpeed = this.moveSpeed * rightClickMultiplier;

            this.centerCoordinate = new Vector2(this.rectForResolution.rect.width / 2, this.rectForResolution.rect.height / 2);

#if UNITY_ANDROID
            //Пересчитать скорость перемещения для андроида.
            this.moveSpeed *= androidSpeedMultiplier;
#else
                //Прямоугольники для отлавливания касания края экрана.
                {
                this.rightSide = CalculateRectFromRectTransform(this.rightSideRectTransform);
                this.leftSide = CalculateRectFromRectTransform(this.leftSideRectTransform);
                this.topSide = CalculateRectFromRectTransform(this.topSideRectTransform);
                this.bottomSide = CalculateRectFromRectTransform(this.bottomSideRectTransform);
            }
#endif
        }

        private void Update()
        {
#if UNITY_ANDROID

#else
            //Прямоугольники для отлавливания касания края экрана.
            {
                if (this.rightSide.Contains(Input.mousePosition))
                {
                    MoveRight();
                }
                else if (this.leftSide.Contains(Input.mousePosition))
                {
                    MoveLeft();
                }
                if (this.topSide.Contains(Input.mousePosition))
                {
                    MoveUp();
                }
                else if (bottomSide.Contains(Input.mousePosition))
                {
                    MoveDown();
                }
            }

            if(this.isRightButtonPressed && Input.GetMouseButtonUp(1))
            {
                this.isRightButtonPressed = false;
            }
            else if(!this.isRightButtonPressed && Input.GetMouseButtonDown(1))
            {
                this.isRightButtonPressed = true;
                this.firsrtMousePositionWithRightDown = Input.mousePosition;
            }
            if (this.isRightButtonPressed)
            {
                Move(this.increasedMoveSpeed * this.distanceOfCursorFromCenterInPercent.x, this.increasedMoveSpeed * this.distanceOfCursorFromCenterInPercent.y);
            }
#endif
        }


        public void OnDrag(PointerEventData eventData)
        {
#if UNITY_ANDROID

            Move(-eventData.delta.x, -eventData.delta.y);

#endif
        }
    }
}
