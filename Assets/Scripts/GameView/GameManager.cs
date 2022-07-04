﻿using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Commands.GameCommands;
using Assets.Scripts.GameModel.Logging;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameView;
using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject mainCameraObjectPrivate = null;
    public GameObject mainCameraObject
    {
        get => this.mainCameraObjectPrivate;
    }

    #region View info

    /// <summary>
    /// Скрипты всех представлений в текущей игре.
    /// </summary>
    private List<GameObject> allViewsInGame = new List<GameObject>();
    /// <summary>
    /// Добавить объект скрипта в список представлений текущей игры.
    /// </summary>
    /// <param name="script"></param>
    public void AddGameViewOriginScript(GameViewOriginScript script)
    {
        this.allViewsInGame.Add(script.gameObject);
    }
    /// <summary>
    /// Разрушить все представления в игре.
    /// </summary>
    private void DestroyAllViewsInGame()
    {
        for (Int32 index = 0; index < this.allViewsInGame.Count; index++)
        {
            if (this.allViewsInGame[index] != null)
                Destroy(this.allViewsInGame[index], 0f);
        }
    }

    #endregion View info

    #region Провайдеры.

    [SerializeField]
    private GraphicPrefabsProvider prefabsProviderPrivate = null;
    public GraphicPrefabsProvider prefabsProvider
    {
        get => this.prefabsProviderPrivate;
    }
    [SerializeField]
    private TreasureProvider treasureProviderPrivate = null;
    public TreasureProvider treasureProvider
    {
        get => this.treasureProviderPrivate;
    }
    [SerializeField]
    private SpriteProvider treasureSpriteProviderPrivate = null;
    public SpriteProvider treasureSpriteProvider
    {
        get => this.treasureSpriteProviderPrivate;
    }
    [SerializeField]
    private UICardWithTreasureSlotProvider uiCardWithTreasureSlotProviderPrivate = null;
    public UICardWithTreasureSlotProvider uiCardWithTreasureSlotProvider
    {
        get => this.uiCardWithTreasureSlotProviderPrivate;
    }
    [SerializeField]
    private PlayerAvatarsProvider playerAvatarsProviderPrivate = null;
    public PlayerAvatarsProvider playerAvatarsProvider
    {
        get => this.playerAvatarsProviderPrivate;
    }

    #endregion Провайдеры.

    #region Модель и представление игры.

    /// <summary>
    /// Модель игры, реализовывает логику взаимодействия всех частей.
    /// </summary>
    private Game gameModelPrivate;
    /// <summary>
    /// Модель игры, реализовывает логику взаимодействия всех частей.
    /// </summary>
    public Game gameModel
    {
        get
        {
            if (this.gameModelPrivate == null)
            {
                LoadAndStartLastGameOrStartNewGame();
            }
            return this.gameModelPrivate;
        }
    }
    /// <summary>
    /// Шаблон, по которому будет воссоздаваться игровое поле.
    /// </summary>
    [SerializeField]
    private FieldView fieldViewTemplate = null;
    /// <summary>
    /// Текущее представление игрового поля.
    /// </summary>
    private FieldView currentFieldView = null;
    /// <summary>
    /// Начать новую игру.
    /// </summary>
    private void StartNewGame()
    {
        PlayerInfo[] playerInfos = new PlayerInfo[]
                        {
                        new PlayerInfo("test1", System.Drawing.Color.Orange),
                        new PlayerInfo("test2", System.Drawing.Color.Red),
                        new PlayerInfo("test3", System.Drawing.Color.Blue),
                        new PlayerInfo("test4", System.Drawing.Color.Purple)
                        };
        GameInfo gameInfo = new GameInfo(playerInfos);

#if UNITY_EDITOR
        gameInfo.cardsShuffleSeed = 1;
        gameInfo.fisrtPlayerNumberSeed = 1;
        gameInfo.cellsShuffleSeed = 1;

#else
                gameInfo.cardsShuffleSeed = Random.Range(-999, 999);
                gameInfo.fisrtPlayerNumberSeed = Random.Range(-999, 999);
                gameInfo.cellsShuffleSeed = Random.Range(-999, 999);
#endif

        this.gameModelPrivate = new Game();
        bool isStartedGame = this.gameModelPrivate.Start(gameInfo);

        if (!isStartedGame)
        {
            LogError("Game not started!");
        }
    }

    #endregion Модель и представление игры.

    #region Pulls



    #endregion Pulls

    #region Сохранение.

    /// <summary>
    /// Объект для сохранения в бинарном формате.
    /// </summary>
    private BinnarySaver saver = new BinnarySaver();
    /// <summary>
    /// Полное имя (путь) для сохранения файла.
    /// </summary>
    private String fullFileNameForSave
    {
        get => Application.persistentDataPath + "\\Last game";
    }
    /// <summary>
    /// Сохранить последнюю игру.
    /// </summary>
    public void SaveLastGame()
    {
        this.saver.Save(this.gameModel.commandKeeper, this.fullFileNameForSave);
    }
    /// <summary>
    /// Загрузить последнюю игру или начать новую, если сохранения нет.
    /// </summary>
    public void LoadAndStartLastGameOrStartNewGame()
    {
        if (!isApplicationQuited)
        {
            GameCommandKeeper gameKeeper = this.saver.Load(this.fullFileNameForSave);
            if (gameKeeper == null)
            {
                StartNewGame();
            }
            else
            {
                this.gameModelPrivate = new Game();
                bool isStartedGame = this.gameModelPrivate.Start(gameKeeper.gameInfo);

                if (!isStartedGame)
                {
                    LogError("Game not started!");
                }
                else
                {
                    gameKeeper.isStartWithFirstCommand = true;
                    while (!gameKeeper.isEmpty)
                    {
                        this.gameModel.ExecuteCommand(gameKeeper.Pop());
                    }
                }
            }

            if (this.currentFieldView != null)
            {
                GameObject.Destroy(this.currentFieldView.gameObject);
            }

            this.currentFieldView = Instantiate(this.fieldViewTemplate);
            this.currentFieldView.Initialize();
        }
    }

    #endregion Сохранение.

    private void Start()
    {
        Application.targetFrameRate = 30;

        //Подключение логирования.
        GameModelLogger.onLogError += LogError;
        GameModelLogger.onLogWarning += LogWarning;
        GameModelLogger.onLogInfo += LogInfo;

        //Новая игра начнется сама при первом вызове this.gameModel
        //Но если до этого момента этого так и не случилось, то стоит начать игру принудительно
        if (this.gameModelPrivate == null)
        {
            StartNewGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveLastGame();
        isApplicationQuitedPrivate = true;
        DestroyAllViewsInGame();
    }
    /// <summary>
    /// Управляющий игрой объект приступил к своему уничтожению.
    /// </summary>
    private static Boolean isApplicationQuitedPrivate = false;
    /// <summary>
    /// Управляющий игрой объект приступил к своему уничтожению.
    /// </summary>
    public static Boolean isApplicationQuited
    {
        get => isApplicationQuitedPrivate;
    }
    private void OnDestroy()
    {
        DestroyAllViewsInGame();
        //Отключение логирования.
        GameModelLogger.onLogError -= LogError;
        GameModelLogger.onLogWarning -= LogWarning;
        GameModelLogger.onLogInfo -= LogInfo;
    }
}
