using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.UI.MainMenuInterface
{

    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Инфо сцены с игрой.
        /// </summary>
        private Scene currentGameScene;
        private void Awake()
        {
            SceneManager.LoadSceneAsync("PrototypeScene", LoadSceneMode.Additive);
            this.currentGameScene = SceneManager.GetSceneByName("PrototypeScene");
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


    }
}
