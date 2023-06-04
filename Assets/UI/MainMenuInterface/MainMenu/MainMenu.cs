using Settings;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

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
        private const string PROTOTIPE_SCENE_NAME = "PrototypeScene";
        [Inject] private MenuManager menuManager;
        [Inject] private GameManager gameManager;
        [Inject] private GeneralSettings generalSettings;
        private void Start()
        {
            SceneManager.LoadSceneAsync(PROTOTIPE_SCENE_NAME, LoadSceneMode.Additive);
            this.currentGameScene = SceneManager.GetSceneByName(PROTOTIPE_SCENE_NAME);
            this.resumeButton.interactable = generalSettings.isThereGameStarted;
            this.menuManager.onMenuChanged += ChangeInteractableResumeLastGameButton;
        }

        private void ChangeInteractableResumeLastGameButton()
        {
            if (menuManager.currentMenuType == MenuType.mainMenu)
            {
                this.resumeButton.interactable = generalSettings.isThereGameStarted;
            }
        }
        public void ResumeLastGame()
        {
            SceneManager.SetActiveScene(this.currentGameScene);
            menuManager.SetActiveMainMenu(MenuType.gameRoot);
            this.gameManager.LoadAndStartLastGameOrStartNewGame();
        }
        public void StartNewGame()
        {
            SceneManager.SetActiveScene(this.currentGameScene);
            menuManager.SetActiveMainMenu(MenuType.gameRoot);
            this.gameManager.StartNewGame();
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
            menuManager.onMenuChanged -= ChangeInteractableResumeLastGameButton;
        }


    }
}
