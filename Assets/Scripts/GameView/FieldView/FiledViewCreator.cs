using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameView
{
    public class FiledViewCreator : MonoBehaviourLogger
    {
        /// <summary>
        /// Шаблон, по которому будет воссоздаваться игровое поле.
        /// </summary>
        [SerializeField] private FieldView fieldViewTemplate = null;

        /// <summary>
        /// Текущее представление игрового поля.
        /// </summary>
        private FieldView currentFieldView = null;
        [Inject] private GameManager gameManager;
        /// <summary>
        /// Создать новое представление игрового поля на основе модели игры.
        /// </summary>
        private void CreateNewFieldView()
        {
            if (this.currentFieldView != null)
            {
                GameObject.Destroy(this.currentFieldView.gameObject);
            }

            this.currentFieldView = InstantiateWithInject(this.fieldViewTemplate, this.transform);
            //this.currentFieldView.Initialize();
        }
        private void Awake()
        {
            this.gameManager.createdNewFieldView += CreateNewFieldView;
        }
    }
}