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
        protected override void Awake()
        {
            GameInterfaceRectanglesDetected.instance.AddGameViewOriginScripts(this);
        }
    }
}