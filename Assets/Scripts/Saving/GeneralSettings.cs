using System;
using UnityEngine;

namespace Settings
{
    public class GeneralSettings : MonoBehaviourLogger
    {
        #region Установка значений в PlayerPrefs и указаное поле.

        private void SetValueInt(string fieldName, ref Int32 field, Int32 value)
        {
            field = value;
            PlayerPrefs.SetInt(fieldName, value);
        }
        private void SetValueBool(string fieldName, ref Boolean field, Boolean value)
        {
            field = value;
            PlayerPrefs.SetInt(fieldName, value ? 1 : 0);
        }
        private Boolean GetValueBool(string fieldName, Boolean defaultValue)
        {
            return PlayerPrefs.GetInt(fieldName, defaultValue ? 1 : 0) == 1;
        }

        #endregion Установка значений в PlayerPrefs и указаное поле.

        #region Продолжение игры.

        private Boolean isThereGameStartedPrivate = false;
        public Boolean isThereGameStarted
        {
            get => this.isThereGameStartedPrivate;
            set => SetValueBool(nameof(this.isThereGameStartedPrivate), ref this.isThereGameStartedPrivate, value);
        }

        #endregion Продолжение игры.

        private void Awake()
        {
            this.isThereGameStartedPrivate = GetValueBool(nameof(this.isThereGameStartedPrivate), false);
        }
    }
}