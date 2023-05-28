﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameView
{
    [Serializable]
    [CreateAssetMenu(fileName = "Sprite Provider", menuName = "Game view/Sprite Provider")]
    public class SpriteProvider : AbstractProvider
    {
        [Serializable]
        private class ObjectWithID
        {
#pragma warning disable CS0649

            public Sprite sprite;
            public Int32 ID;

#pragma warning restore CS0649
        }

        #region Объекты в провайдере.

        [SerializeField]
        private ObjectWithID[] resourcesList = new ObjectWithID[0];
        private Dictionary<Int32, Sprite> resourcesDictionaryField = null;
        private Dictionary<Int32, Sprite> resourcesDictionary
        {
            get
            {
                if (this.resourcesDictionaryField == null)
                {
                    this.resourcesDictionaryField = new Dictionary<Int32, Sprite>(this.resourcesList.Length);
                    foreach (ObjectWithID obj in this.resourcesList)
                    {
                        this.resourcesDictionaryField.Add(obj.ID, obj.sprite);
                    }
                }
                return this.resourcesDictionaryField;
            }
        }

        #endregion Объекты в провайдере.

        #region Действия.


        /// <summary>
        /// Получить клон картинки сокровища с указанным id.
        /// </summary>
        /// <param name="treasureID"></param>
        /// <returns></returns>
        public Sprite GetSpriteClone(Int32 treasureID)
        {
            if (this.resourcesDictionary.ContainsKey(treasureID))
            {
                return this.resourcesDictionary[treasureID];
            }
            else
            {
                Debug.LogError("In " + nameof(TreasureProvider) + " not found treasure with ID: " + treasureID + "!");
                return null;
            }
        }

        #endregion Действия.
    }
}
