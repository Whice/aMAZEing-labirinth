using System;
using UnityEngine;

namespace Assets.UI.MainMenuInterface
{
    /// <summary>
    /// Класс для корня одного из меню.
    /// </summary>
    public class MenuRootScript : MonoBehaviourLogger
    {
        /// <summary>
        /// Тип меню для этого корневого объекта.
        /// </summary>
        [SerializeField]
        private MenuType rootTypePrivate = MenuType.unknow;
        /// <summary>
        /// Тип меню для этого корневого объекта.
        /// </summary>
        public MenuType rootType
        { 
            get => this.rootTypePrivate;
        }

        /// <summary>
        /// Показать все объекты принадлежащие этому кроневому объекту.
        /// </summary>
        /// <param name="isActive">Показать или нет.</param>
        public void SetActive(Boolean isActive)
        {
            this.gameObject.SetActive(isActive);
        }

        [SerializeField]
        private Boolean isActiveInAwake = false;
        private void Awake()
        {
            SetActive(this.isActiveInAwake);
            MenuManager.instance.AddMenuRootScript(this);
        }
    }
}
