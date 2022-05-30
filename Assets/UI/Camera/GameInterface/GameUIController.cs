using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Player;
using System;
using TMPro;
using UI;
using UnityEngine;

namespace Assets.UI.Camera.GameInterface
{
    /// <summary>
    /// Класс для контролирования всех элементов UI во время игры.
    /// </summary>
    public class GameUIController: GameUIOriginScript
    {
        /// <summary>
        /// Пропустить ход для текущего игрока.
        /// </summary>
        public void PlayerMissMove()
        {
            this.gameModel.PlayerMissMove();
        }

        #region Конец игры.

        [Header("End Game")]
        [SerializeField]
        private TextMeshProUGUI tmpForWinnerTable = null;
        [SerializeField]
        private GameObject endGameObject = null;
        private void ShowGameEndTable()
        {
            String winnerTable = "Позиции игроков:\n";
            GamePlayer[] winners = this.gameModel.GetWinners();
            Int32 numberWinner = 0;
            foreach (GamePlayer winner in winners)
            {
                ++numberWinner;
                winnerTable += numberWinner + ". " + winner.name + "\n";
            }

            this.tmpForWinnerTable.text = winnerTable;
            this.endGameObject.SetActive(true);
        }

        #endregion Конец игры.

        protected override void Awake()
        {
            base.Awake();
            this.gameModel.OnGameEnded += ShowGameEndTable;
        }
        protected override void OnDestroy()
        {
            this.gameModel.OnGameEnded -= ShowGameEndTable;
            base.OnDestroy();
        }
    }
}
