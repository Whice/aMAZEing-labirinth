using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// ������ ��� ����������� ������ �� �������� ����.
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        /// <summary>
        /// ������ � ������� ������.
        /// </summary>
        public new Transform camera;
        /// <summary>
        /// �������� ����������� ������.
        /// </summary>
        [SerializeField]
        private Single moveSpeed = 0.05f;

        /// <summary>
        /// ������������� ������� ������� � ������ �������.
        /// </summary>
        private Rect rightSide;
        /// <summary>
        /// ������ �� ������� ������� ������� � ������ �������. 
        /// </summary>
        [SerializeField]
        private RectTransform rightSideRectTransform;
        /// <summary>
        /// ������������� ������� ������� � ����� �������.
        /// </summary>
        private Rect leftSide;
        /// <summary>
        /// ������ �� ������� ������� ������� � ����� �������. 
        /// </summary>
        [SerializeField]
        private RectTransform leftSideRectTransform;
        /// <summary>
        /// ������������� ������� ������� � ������� �������.
        /// </summary>
        private Rect topSide;
        /// <summary>
        /// ������ �� ������� ������� ������� � ������� �������. 
        /// </summary>
        [SerializeField]
        private RectTransform topSideRectTransform;
        /// <summary>
        /// ������������� ������� ������� � ������ �������.
        /// </summary>
        private Rect bottomSide;
        /// <summary>
        /// ������ �� ������� ������� ������� � ������ �������. 
        /// </summary>
        [SerializeField]
        private RectTransform bottomSideRectTransform;

        /// <summary>
        /// ������������� ������ �� ������� ������� � ������������� �������.
        /// </summary>
        /// <param name="sideRectTransform"></param>
        /// <returns></returns>
        private Rect CalculateRectFromRectTransform(RectTransform sideRectTransform)
        {
            return new Rect(
                new Vector2
                (
                    sideRectTransform.position.x - sideRectTransform.rect.size.x / 2,
                    sideRectTransform.position.y - sideRectTransform.rect.size.y / 2
                    ),
                sideRectTransform.rect.size);
        }



        private void Awake()
        {
            this.rightSide = CalculateRectFromRectTransform(this.rightSideRectTransform);
            this.leftSide = CalculateRectFromRectTransform(this.leftSideRectTransform);
            this.topSide = CalculateRectFromRectTransform(this.topSideRectTransform);
            this.bottomSide = CalculateRectFromRectTransform(this.bottomSideRectTransform);
        }
        private void Update()
        {
            if (this.rightSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x + this.moveSpeed,
                    this.camera.position.y,
                    this.camera.position.z
                    );
            }
            else if (this.leftSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x - this.moveSpeed,
                    this.camera.position.y,
                    this.camera.position.z
                    );
            }
            if (this.topSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x,
                    this.camera.position.y,
                    this.camera.position.z + this.moveSpeed
                    );
            }
            else if (bottomSide.Contains(Input.mousePosition))
            {
                this.camera.position = new Vector3
                    (
                    this.camera.position.x,
                    this.camera.position.y,
                    this.camera.position.z - this.moveSpeed
                    );
            }
        }
    }
}
