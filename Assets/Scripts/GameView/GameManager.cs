using Assets.Scripts.GameModel;
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
    private GraphicPrefabsProvider prefabsProviderPrivate=null;
    public GraphicPrefabsProvider prefabsProvider
    {
        get=>this.prefabsProviderPrivate;
    }
    [SerializeField]
    private TreasureProvider treasureProviderPrivate = null;
    public TreasureProvider treasureProvider
    {
        get=>this.treasureProviderPrivate;
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
                this.gameModelPrivate = new Game();
                bool isStartedGame = this.gameModelPrivate.Start
                    (
                    new PlayerInfo[]
                        {
                new PlayerInfo("test1", System.Drawing.Color.Black),
                new PlayerInfo("test2", System.Drawing.Color.Yellow)
                        },
                    out var message);

                if (!isStartedGame)
                {
                    Debug.LogError("Game not started:\n" + message);
                }
            }
            return this.gameModelPrivate;
        }
    }



    private void Awake()
    {
        
    }
}
