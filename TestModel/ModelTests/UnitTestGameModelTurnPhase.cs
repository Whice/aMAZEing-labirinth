using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Тесты перечисления фаз во время хода игрока.
    /// </summary>
    public class UnitTestGameModelTurnPhase
    {
        /// <summary>
        /// Правильный переход на следующую фазу.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_SuccessfullCreatePinnedCells()
        {
            TurnPhase phase = TurnPhase.movingCell;

            phase = phase.GetNextPhase();
            Assert.Equal(TurnPhase.movingAvatar, phase);

            phase = phase.GetNextPhase();
            Assert.Equal(TurnPhase.movingCell, phase);
        }
    }
}
