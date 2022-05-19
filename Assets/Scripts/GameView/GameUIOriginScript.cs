namespace UI
{
    public class GameUIOriginScript : GameViewOriginScript
    {
        protected virtual void Awake()
        {
            GameInterfaceRectanglesDetected.instance.AddGameViewOriginScripts(this);
            LogInfo(this.gameObject.name);
        }
    }
}