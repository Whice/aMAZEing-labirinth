namespace Assets.Scripts.GameModel.PlayingField
{
    /// <summary>
    /// Стороны поля.
    /// <br/>Вверх/вниз соответсвенно равны 1/-1 чтобы в сумме друг с другом давать 0, но не с дургими.
    /// <br/>Вправо/влево соответсвенно равны 2/-2 чтобы в сумме друг с другом давать 0, но не с дургими.
    /// </summary>
    public enum FieldSide
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow = 0,
        /// <summary>
        /// Верхняя сторона.
        /// </summary>
        up = 1,
        /// <summary>
        /// Правая сторона.
        /// </summary>
        right = 2,
        /// <summary>
        /// Нижняя сторона.
        /// </summary>
        down = -1,
        /// <summary>
        /// Левая сторона.
        /// </summary>
        left = -2,
    }
}
