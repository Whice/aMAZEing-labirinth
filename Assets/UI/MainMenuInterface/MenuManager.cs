using Assets.UI.MainMenuInterface;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuManager : MonoSingleton<MenuManager>
{
    #region Активность корневых объектов (какие окна меню показываются).

    private Scene mainMenuScenePrivate;
    public Scene mainMenuScene
    {
        get => this.mainMenuScenePrivate;
    }

    /// <summary>
    /// Текущее открытое меню.
    /// </summary>
    private MenuRootScript currentMenuRootScript = null;
    /// <summary>
    /// Набор всех меню, которые есть в игре.
    /// </summary>
    private Dictionary<MenuType, MenuRootScript> menuRootScripts = new Dictionary<MenuType, MenuRootScript>();
    /// <summary>
    /// Добавить новое меню.
    /// </summary>
    /// <param name="menuRootScript"></param>
    public void AddMenuRootScript(MenuRootScript menuRootScript)
    {
        if (menuRootScript.rootType == MenuType.unknow)
        {
            LogError("menuRootScript.rootType cannot be unknow!");
        }
        if (this.menuRootScripts.ContainsKey(menuRootScript.rootType))
        {
            LogError("Type " + menuRootScript.rootType.ToString() + " menus already exist!");
            return;
        }

        this.menuRootScripts.Add(menuRootScript.rootType, menuRootScript);
        if (this.currentMenuRootScript == null)
        {
            this.currentMenuRootScript = menuRootScript;
        }
    }
    /// <summary>
    /// Установить видимым меню указанного типа.
    /// Предыдущее меню скрывается.
    /// </summary>
    /// <param name="menuType">Тип меню.</param>
    public void SetActiveMainMenu(MenuType menuType)
    {
        this.currentMenuRootScript.SetActive(false);
        this.currentMenuRootScript = this.menuRootScripts[menuType];
        this.currentMenuRootScript.SetActive(true);
    }
    /// <summary>
    /// Скрыть все меню.
    /// </summary>
    public void UnactiveAllMenus()
    {
        this.currentMenuRootScript.SetActive(false);
    }

    #endregion Активность корневых объектов (какие окна меню показываются).

    private void Awake()
    {
        this.mainMenuScenePrivate = SceneManager.GetSceneByName("MainMenu");
    }
}
