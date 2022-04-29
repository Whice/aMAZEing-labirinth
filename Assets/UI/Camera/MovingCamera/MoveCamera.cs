using System;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform camera;
    public Single moveSpeed = 0.5f;

    private Rect rightSide;
    public RectTransform rightSideRectTransform;
    private Rect leftSide;
    public RectTransform leftSideRectTransform;
    private Rect topSide;
    public RectTransform topSideRectTransform;
    private Rect bottomSide;
    public RectTransform bottomSideRectTransform;

    private Rect CalculateRectFromRectTransform(RectTransform rectTransform)
    {
        return new Rect(
            new Vector2
            (
                rectTransform.position.x - rectTransform.rect.size.x / 2,
                rectTransform.position.y - rectTransform.rect.size.y / 2
                ),
            rectTransform.rect.size);
    }

    private void Awake()
    {
        rightSide = CalculateRectFromRectTransform(rightSideRectTransform);
        leftSide = CalculateRectFromRectTransform(leftSideRectTransform);
        topSide = CalculateRectFromRectTransform(topSideRectTransform);
        bottomSide = CalculateRectFromRectTransform(bottomSideRectTransform);
    }
    private void Update()
    {
        if (rightSide.Contains(Input.mousePosition))
        {
            Debug.Log("Mouse in right rect");
            camera.position = new Vector3(camera.position.x+moveSpeed, camera.position.y, camera.position.z);
        }
        else if (leftSide.Contains(Input.mousePosition))
        {
            Debug.Log("Mouse in left rect");
            camera.position = new Vector3(camera.position.x-moveSpeed, camera.position.y, camera.position.z);
        }
        if (topSide.Contains(Input.mousePosition))
        {
            Debug.Log("Mouse in top rect");
            camera.position = new Vector3(camera.position.x, camera.position.y, camera.position.z+ moveSpeed);
        }
        else if (bottomSide.Contains(Input.mousePosition))
        {
            Debug.Log("Mouse in bottom rect");
            camera.position = new Vector3(camera.position.x, camera.position.y, camera.position.z- moveSpeed);
        }
    }
}
