using System;

namespace Assets.Scripts.Extensions
{
    /// <summary>
    /// Структура, которая хранит два байта в одном.
    /// Оба байта не должны быть отрицательные и больше 4х бит, т.е. больше 7.
    /// </summary>
    public struct TwoBytesInOneKeeper
    {
        /// <summary>
        /// Задать два байта для хранения и создать хранилище.
        /// </summary>
        /// <param name="firstValue">Первый байт.</param>
        /// <param name="secondValue">Второй байт.</param>
        public TwoBytesInOneKeeper(byte firstValue, byte secondValue)
        {
            this.twoValues = 0;
            this.firstValue = firstValue;
            this.secondValue = secondValue;
        }
        /// <summary>
        /// Левые четыре бита, котороые равны 1.
        /// </summary>
        private const byte leftFourBits = 0b11110000;
        /// <summary>
        /// Правые четыре бита, котороые равны 1.
        /// </summary>
        private const byte rightFourBits = 0b00001111;
        /// <summary>
        /// Оба значения.
        /// </summary>
        private byte twoValues;
        /// <summary>
        /// Первый байт.
        /// </summary>
        public byte firstValue
        {
            get
            {
                return (byte)(twoValues & rightFourBits);
            }
            set
            {
                CheckValidValue(value);
                twoValues &= leftFourBits;
                twoValues |= value;
            }
        }
        /// <summary>
        /// Второй байт.
        /// </summary>
        public byte secondValue
        {
            get
            {
                return (byte)(twoValues >> 4);
            }
            set
            {
                CheckValidValue(value);
                twoValues &= rightFourBits;
                twoValues = (byte)(twoValues | (value << 4));
            }
        }
        /// <summary>
        /// Проверить значение на то, что оно влезает в 4 бита.
        /// Если оно не влезает, вызвать исключение.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckValidValue(byte value)
        {
            if (value < 0 || value > 7)
            {
                throw new ArgumentException("Value can be only in range 0..7!");
            }
        }
    }
}
