namespace Assets.Scripts.GameModel.TurnPhaseAndExtensions
{
    /// <summary>
    /// Фазы во время хода одного из игроков.
    /// </summary>
    public enum TurnPhase
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow=0,
        /// <summary>
        /// Передвижение ячеек.
        /// </summary>
        movingCell=1,
        /// <summary>
        /// Передвижение аватара(фишки, фигурки) игрока.
        /// </summary>
        movingAvatar=2
    }
}
