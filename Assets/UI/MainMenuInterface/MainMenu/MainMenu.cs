﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.UI.MainMenuInterface
{
    /// <summary>
    /// Скрипт главного меню.
    /// </summary>
    public class MainMenu : MonoBehaviourLogger
    {
        /// <summary>
        /// Компонент кнопки "продолжить".
        /// </summary>
        [SerializeField] private Button resumeButton = null;
        /// <summary>
        /// Инфо сцены с игрой.
        /// </summary>
        private Scene currentGameScene;
        private void Awake()
        {
            SceneManager.LoadSceneAsync("PrototypeScene", LoadSceneMode.Additive);
            this.currentGameScene = SceneManager.GetSceneByName("PrototypeScene");
            this.resumeButton.interactable = GeneralSettings.instance.isThereGameStarted;
            MenuManager.instance.onMenuChanged += ChangeInteractableResumeLastGameButton;
        }

        private void ChangeInteractableResumeLastGameButton()
        {
            if (MenuManager.instance.currentMenuType == MenuType.mainMenu)
            {
                this.resumeButton.interactable = GeneralSettings.instance.isThereGameStarted;
            }
        }
        public void ResumeLastGame()
        {
            SceneManager.SetActiveScene(this.currentGameScene);
            MenuManager.instance.SetActiveMainMenu(MenuType.gameRoot);
            GameManager.instance.LoadAndStartLastGameOrStartNewGame();
        }
        public void StartNewGame()
        {
            SceneManager.SetActiveScene(this.currentGameScene);
            MenuManager.instance.SetActiveMainMenu(MenuType.gameRoot);
            GameManager.instance.StartNewGame();
        }
        public void OpenHowToPlay()
        {

        }
        public void OpenSetting()
        {

        }
        public void QuitApplication()
        {
            Application.Quit();
        }
        private void OnDestroy()
        {
            MenuManager.instance.onMenuChanged -= ChangeInteractableResumeLastGameButton;
        }


    }
}
