using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using Assets.UI.MainMenuInterface;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
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
            if(this.gameModel.currentPhase == TurnPhase.movingAvatar)
                this.gameModel.PlayerMissMove();
        }

        #region Переход в главное меню.

        private MenuManager menuManager
        {
            get => MenuManager.instance;
        }
        public void OpenMainMenu()
        {
            SceneManager.SetActiveScene(this.menuManager.mainMenuScene);
            this.menuManager.SetActiveMainMenu(MenuType.mainMenu);            
        }

        #endregion Переход в главное меню.

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
            this.isShouldInterruptPressesInWorld = false;
            this.gameModel.OnGameEnded += ShowGameEndTable;
        }
        protected override void OnDestroy()
        {
            this.gameModel.OnGameEnded -= ShowGameEndTable;
            base.OnDestroy();
        }
    }
}
