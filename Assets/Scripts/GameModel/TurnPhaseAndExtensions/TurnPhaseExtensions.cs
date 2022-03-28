namespace Assets.Scripts.GameModel.TurnPhaseAndExtensions
{
    public static class TurnPhaseExtensions
    {
        /// <summary>
        /// Получить следующую фазу.
        /// </summary>
        /// <param name="currentPhase">Текущая фаза.</param>
        /// <returns></returns>
        public static TurnPhase GetNextPhase(this TurnPhase currentPhase)
        {
            currentPhase = currentPhase + 1;
            if (currentPhase > TurnPhase.movingAvatar)
            {
                currentPhase = TurnPhase.movingCell;
            }
            return currentPhase;
        }
    }
}
