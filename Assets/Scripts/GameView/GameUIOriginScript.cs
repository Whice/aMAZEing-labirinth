using Assets.Scripts.GameModel.Player;

namespace UI
{
    public class GameUIOriginScript : GameViewOriginScript
    {
        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        protected GamePlayer currentPlayer
        {
            get => this.gameModel.currentPlayer;
        }
        protected virtual void Awake()
        {
            GameInterfaceRectanglesDetected.instance.AddGameViewOriginScripts(this);
            LogInfo(this.gameObject.name);
        }
    }
}