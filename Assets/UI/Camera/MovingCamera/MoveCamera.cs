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
        private Single moveSpeed = 1.5f;
        /// <summary>
        /// Увеличенная скорость перемещения камеры. Для движения правой кнопкой мыши.
        /// </summary>
        private Single increasedMoveSpeed;
        /// <summary>
        /// Множитель при нажатии правой кнопки мыши.
        /// </summary>
        private const Int32 RIGHT_CLICK_MULTIPLIER = 5;
        /// <summary>
        /// Правая кнопка мыши зажата.
        /// </summary>
        private Boolean isRightButtonPressed = false;
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
        private const Single ANDROID_SPEED_MIULTIPLUER = 0.3f;

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
            Single newPosX = this.camera.position.x + xShift * this.moveSpeed * Time.deltaTime;
            Single newPosY = this.camera.position.z + zShift * this.moveSpeed * Time.deltaTime;
            
            //Проверка выхода за указанные границы для камеры.
            if (newPosX < this.bordersForMinMaxX.x || newPosX > this.bordersForMinMaxX.y)
            {
                newPosX = this.camera.position.x;
            }
            if (newPosY < this.bordersForMinMaxY.x || newPosY > this.bordersForMinMaxY.y)
            {
                newPosY = this.camera.position.z;
            }

            this.camera.position = new Vector3
                    (
                    newPosX,
                    this.camera.position.y,
                   newPosY
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

        #region Границы, за которые нельзя выходить.

        [SerializeField]
        private Transform objectsForBorders = null;
        private Vector2 bordersForMinMaxX = Vector2.zero;
        private Vector2 bordersForMinMaxY = Vector2.zero;

        #endregion Границы, за которые нельзя выходить.

        private void Awake()
        {
            //Расчитать значение скорости для нестандартного разрешения (не 1920х1080)
            this.moveSpeed *= this.resolutionScaling;
            this.increasedMoveSpeed = this.moveSpeed * RIGHT_CLICK_MULTIPLIER;

            this.centerCoordinate = new Vector2(this.rectForResolution.rect.width / 2, this.rectForResolution.rect.height / 2);

            //посчитать границы, закоторые не должна выходить камера.
            //Рассчет приведен для невращающейся камеры.
            if (this.objectsForBorders != null)
            {
                Single borderShiftX = this.objectsForBorders.localScale.x * this.camera.position.y;
                this.bordersForMinMaxX = new Vector2(
                    this.objectsForBorders.position.x - borderShiftX,
                    this.objectsForBorders.position.x + borderShiftX
                    );
                Single borderShiftY = this.objectsForBorders.localScale.y * this.camera.position.y/3;
                this.bordersForMinMaxY = new Vector2(
                    this.objectsForBorders.position.y - borderShiftY,
                    this.objectsForBorders.position.y + borderShiftY
                    );
            }
            else
            {
                this.bordersForMinMaxX = new Vector2(Single.MinValue, Single.MaxValue);
                this.bordersForMinMaxY = new Vector2(Single.MinValue, Single.MaxValue);
            }

#if UNITY_ANDROID
            //Пересчитать скорость перемещения для андроида.
            this.moveSpeed *= ANDROID_SPEED_MIULTIPLUER;
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
