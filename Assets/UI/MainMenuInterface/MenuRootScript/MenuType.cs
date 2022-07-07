namespace Assets.UI.MainMenuInterface
{
    /// <summary>
    /// Типы корневых объектов меню.
    /// </summary>
    public enum MenuType : byte
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow = 0,
        /// <summary>
        /// Главное меню.
        /// </summary>
        mainMenu = 1,
        /// <summary>
        /// Меню "как играть".
        /// </summary>
        howToPlay = 2,
        /// <summary>
        /// Меню настроек игры.
        /// </summary>
        settings = 3,


        /// <summary>
        /// Не меню, но помогает удобно с ними переключатся 
        /// т.к. вход в любое меню должен скрыть игру.
        /// </summary>
        gameRoot = 254
    }
}
