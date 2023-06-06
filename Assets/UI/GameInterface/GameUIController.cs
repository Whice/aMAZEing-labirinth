using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using Assets.UI.MainMenuInterface;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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

        #region Переход в главное меню и из него.

        /// <summary>
        /// Сокращенная ссылка на главный управляющий скрипт меню.
        /// </summary>
        [Inject] private MenuManager menuManager;
        /// <summary>
        /// Открыть главное меню.
        /// </summary>
        public void OpenMainMenu()
        {
            this.gameManager.SaveLastGame();
            SceneManager.SetActiveScene(this.menuManager.mainMenuScene);
            this.menuManager.SetActiveMainMenu(MenuType.mainMenu);            
        }
        /// <summary>
        /// Bнициализируемые скрипты.
        /// </summary>
        [SerializeField]
        private GameUIOriginScript[] initializableUIScripts = new GameUIOriginScript[0];
        /// <summary>
        ///  Инициализировать UI скрипты, которые были заданы для инициализации.
        /// </summary>
        public void InitializeUIScripts()
        {
            foreach (GameUIOriginScript script in this.initializableUIScripts)
            {
                script.Initialize();
            }
        }

        #endregion Переход в главное меню и из него.

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


        protected override void Subscribe()
        {
            base.Subscribe();
            this.gameModel.OnGameEnded += ShowGameEndTable;
        }
        protected override void Unsubscribe()
        {
            this.gameModel.OnGameEnded -= ShowGameEndTable;
            base.Unsubscribe();
        }
        public override void Initialize()
        {
            base.Initialize();
            this.isShouldInterruptPressesInWorld = false;
        }

        private void Start()
        {
            InitializeUIScripts();
        }
    }
}
