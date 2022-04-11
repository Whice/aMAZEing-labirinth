namespace Assets.Scripts.GameModel.PlayingField.FieldCells
{
    /// <summary>
    /// Тип ячейки.
    /// Хранит информацию о том, сколько проходов у нее и как они расположены.
    /// <br/>corner = 1,
    /// <br/>line = 2,
    /// <br/>threeDirection = 3
    /// </summary>
    public enum CellType : byte
    {
        /// <summary>
        /// Неизвестно.
        /// </summary>
        unknown = 0,
        /// <summary>
        /// Уголок. Два прохода.
        /// </summary>
        corner = 1,
        /// <summary>
        /// Линия. Два прохода.
        /// </summary>
        line = 2,
        /// <summary>
        /// Три прохода.
        /// </summary>
        threeDirection = 3
    }
}
