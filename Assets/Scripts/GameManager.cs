using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Commands;
using Assets.Scripts.GameModel.Logging;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameView;
using Assets.Scripts.Saving;
using Settings;
using SummonEra.RxEvents;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

/// <summary>
/// Главный управляющий скрипт игры.
/// </summary>
public class GameManager : MonoBehaviourLogger
{
    #region View info

    /// <summary>
    /// Скрипты всех представлений в текущей игре.
    /// </summary>
    private List<GameViewOriginScript> allViewsInGame = new List<GameViewOriginScript>();
    /// <summary>
    /// Добавить объект скрипта в список представлений текущей игры.
    /// </summary>
    /// <param name="script"></param>
    public void AddGameViewOriginScript(GameViewOriginScript script)
    {
        this.allViewsInGame.Add(script);
    }
    /// <summary>
    /// Разрушить все представления в игре.
    /// </summary>
    private void DestroyAllViewsInGame()
    {
        for (Int32 index = 0; index < this.allViewsInGame.Count; index++)
        {
            if (this.allViewsInGame[index] != null)
                Destroy(this.allViewsInGame[index].gameObject);
        }
        this.allViewsInGame.Clear();
    }

    #endregion View info

    #region Провайдеры.
    [Header("Providers")]
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
    /// Инициализировать игру, интерфейс и т.п.
    /// </summary>
    private void InitializeGame()
    {
        CreateNewFieldView();
    }
    /// <summary>
    /// Начать новую игру.
    /// </summary>
    public void StartNewGame()
    {
        PlayerInfo[] playerInfos = new PlayerInfo[]
                        {
                        new PlayerInfo("test1", System.Drawing.Color.Purple),
                        new PlayerInfo("test2", System.Drawing.Color.Yellow)
                        };
        GameInfo gameInfo = new GameInfo(playerInfos);

#if UNITY_EDITOR
                gameInfo.cardsShuffleSeed = 1;
                gameInfo.fisrtPlayerNumberSeed = 1;
                gameInfo.cellsShuffleSeed = 1;
#else
                gameInfo.cardsShuffleSeed = UnityEngine.Random.Range(-999, 999);
                gameInfo.fisrtPlayerNumberSeed = UnityEngine.Random.Range(-999, 999);
                gameInfo.cellsShuffleSeed = UnityEngine.Random.Range(-999, 999);
#endif

        this.gameModelPrivate = new Game();
        bool isStartedGame = this.gameModelPrivate.Start(gameInfo);

        if (!isStartedGame)
        {
            LogError("Game not started!");
            return;
        }

        InitializeGame();
        generalSettings.isThereGameStarted = true;
        this.gameModelPrivate.OnGameEnded += EndGame;
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
    /// Создать новое представление игрового поля на основе модели игры.
    /// </summary>
    public event Action createdNewFieldView;
    /// <summary>
    /// Создать новое представление игрового поля на основе модели игры.
    /// </summary>
    private void CreateNewFieldView()
    {
        this.createdNewFieldView?.Invoke();
    }
    /// <summary>
    /// Загрузить последнюю игру или начать новую, если сохранения нет.
    /// </summary>
    public void LoadAndStartLastGameOrStartNewGame()
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
            InitializeGame();
        }
    }

    #endregion Сохранение.

    #region Общие настройки.

    [Inject] private GeneralSettings generalSettings;
    /// <summary>
    /// Выполнить действия, которые предназначены для конца игры.
    /// </summary>
    private void EndGame()
    {
        generalSettings.isThereGameStarted = false;
    }

    #endregion Общие настройки.

    private CompositeDisposable disposables;
    [Inject]private DiContainer diContainer;
    private void Awake()
    {
        disposables = new CompositeDisposable();
        disposables.Subscribe<TestRxEvent>(IRxMsgmsg => { Debug.Log(IRxMsgmsg.message); });
        List<AbstractProvider> providers = new List<AbstractProvider>();
        providers.Add(this.prefabsProviderPrivate);
        providers.Add(this.treasureProviderPrivate);
        providers.Add(this.treasureSpriteProviderPrivate);
        providers.Add(this.uiCardWithTreasureSlotProviderPrivate);
        providers.Add(this.playerAvatarsProviderPrivate);
        foreach (AbstractProvider provider in providers)
        {
            provider.diContainer = this.diContainer;
        }
    }
    private void Start()
    {
        //Application.targetFrameRate = 30;

        //Подключение логирования.
        GameModelLogger.onLogError += LogError;
        GameModelLogger.onLogWarning += LogWarning;
        GameModelLogger.onLogInfo += LogInfo;

        //Новая игра начнется сама при первом вызове this.gameModel
        //Но если до этого момента этого так и не случилось, то стоит начать игру принудительно
        /*if (this.gameModelPrivate == null)
        {
            StartNewGame();
        }*/
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
