﻿using Assets.UI.MainMenuInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Главный управляющий скрипт меню.
/// </summary>
public class MenuManager : MonoBehaviourLogger
{
    #region Активность корневых объектов (какие окна меню показываются).

    /// <summary>
    /// Сцена главного меню.
    /// </summary>
    private Scene mainMenuScenePrivate;
    /// <summary>
    /// Сцена главного меню.
    /// </summary>
    public Scene mainMenuScene
    {
        get => this.mainMenuScenePrivate;
    }
    /// <summary>
    /// Событие смены (открытия) меню.
    /// </summary>
    public event Action onMenuChanged;
    /// <summary>
    /// Текущий тип меню.
    /// </summary>
    public MenuType currentMenuType { get; private set; }

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
    /// Получить ссылку на меню указанного типа.
    /// </summary>
    public MenuRootScript GetMenuByType(MenuType type)
    {
        if (this.menuRootScripts.ContainsKey(type))
        {
            return this.menuRootScripts[type];
        }
        else
        {
            return null;
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
        this.currentMenuType = menuType;
        this.currentMenuRootScript.SetActive(true);
        this.onMenuChanged?.Invoke();
    }
    /// <summary>
    /// Скрыть все меню.
    /// </summary>
    public void UnactiveAllMenus()
    {
        this.currentMenuRootScript.SetActive(false);
        this.currentMenuType = MenuType.unknow;
        this.onMenuChanged?.Invoke();
    }

    #endregion Активность корневых объектов (какие окна меню показываются).

    private void Awake()
    {
        this.mainMenuScenePrivate = SceneManager.GetSceneByName("MainMenu");
        Screen.SetResolution(1280, 720, false);
    }
}