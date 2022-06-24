using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Logging;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameView;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject mainCameraObjectPrivate = null;
    public GameObject mainCameraObject
    {
        get => this.mainCameraObjectPrivate;
    }

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
                PlayerInfo[] playerInfos = new PlayerInfo[]
                        {
                        new PlayerInfo("test1", System.Drawing.Color.Orange),
                        new PlayerInfo("test2", System.Drawing.Color.Red)
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
                bool isStartedGame = this.gameModelPrivate.Start(gameInfo, out var message);

                if (!isStartedGame)
                {
                    Debug.LogError("Game not started:\n" + message);
                }
            }
            return this.gameModelPrivate;
        }
    }

    #region Pulls



    #endregion Pulls


    private void Awake()
    {
        Application.targetFrameRate = 30;

        //Подключение логирования.
        GameModelLogger.onLogError += LogError;
        GameModelLogger.onLogWarning += LogWarning;
        GameModelLogger.onLogInfo += LogInfo;
    }

    private void OnDestroy()
    {
        //Отключение логирования.
        GameModelLogger.onLogError -= LogError;
        GameModelLogger.onLogWarning -= LogWarning;
        GameModelLogger.onLogInfo -= LogInfo;
    }
}
