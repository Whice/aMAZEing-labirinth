using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Commands.GameCommands;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Saving
{
    /// <summary>
    /// Предоставляет инетерфейс для разного рода сохранений.
    /// </summary>
    public class BinnarySaver
    {
        /// <summary>
        /// Расширение сохраняемого файла.
        /// </summary>
        private const String FILE_EXTENSION = ".ALBS";
        private BinaryFormatter bformatter;
        private MemoryStream streamReader;
        /// <summary>
        /// Загрузить данные из постоянной памяти.
        /// </summary>
        /// <param name="filePathAndName">Путь к сохранению.</param>
        /// <returns></returns>
        public GameCommandKeeper Load(String filePathAndName)
        {
            if (File.Exists(filePathAndName + FILE_EXTENSION))
            {
                byte[] data = File.ReadAllBytes(filePathAndName + FILE_EXTENSION);
                this.streamReader = new MemoryStream(data);
                this.bformatter = new BinaryFormatter();
                GameCommandKeeper gameKeeper = (GameCommandKeeper)bformatter.Deserialize(this.streamReader);
                this.streamReader.Close();
                return gameKeeper;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Сохранить данные в постоянную память.
        /// </summary>
        /// <param name="gameInfo">Данные для сохранения.</param>
        /// <param name="filePathAndName">Путь к сохранению.</param>
        public void Save(GameCommandKeeper gameKeeper, String filePathAndName)
        {
            this.bformatter = new BinaryFormatter();
            this.streamReader = new MemoryStream();
            bformatter.Serialize(streamReader, gameKeeper);
            byte[] binnaryData = streamReader.ToArray();
            File.WriteAllBytes(filePathAndName + FILE_EXTENSION, binnaryData);
            streamReader.Close();
        }
    }
}
