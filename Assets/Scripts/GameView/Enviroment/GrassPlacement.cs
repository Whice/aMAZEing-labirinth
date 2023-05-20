using UnityEngine;

public class GrassPlacement : ObjectPlacement
{
    [SerializeField] private int materialsVariantsCount = 3;
    private Material GetMaterialFromGameObject(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            return renderer.material;
        }

        return null;
    }
    public void SetMaterialToGameObject(GameObject gameObject, Material material)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }
    private float GetPointForIndex(int index)
    {
        float border = 3.14f;
        float maxValue = border * 2;
        float value = maxValue * index / this.materialsVariantsCount;
        return value - border;
    }
    protected override void SetObjectsForCreate()
    {
        base.SetObjectsForCreate();
        Material material = GetMaterialFromGameObject(this.objectPrefab);
        if (material != null)
        {
            for (int i = 0; i < this.materialsVariantsCount; i++)
            {
                GameObject go = Instantiate(this.objectPrefab);
                material.SetFloat("_MovementReferencePoint", GetPointForIndex(i));
                SetMaterialToGameObject(go, material);
                objectsForCreate.Add(go);
            }
        }
    }
}
