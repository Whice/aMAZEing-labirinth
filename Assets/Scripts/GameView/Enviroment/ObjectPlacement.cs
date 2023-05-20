using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] protected GameObject objectPrefab;
    [SerializeField] private Transform boundsTransform;
    [SerializeField] private float density = 1f;
    private List<GameObject> createdObjects= new List<GameObject>();

    public int seed = 0;
    private System.Random random;
    protected float GetRandomNumberByRange(float min, float max)
    {
        return (float)this.random.NextDouble() * (max - min) + min;
    }

    private int objectForCreateIndex = 0;
    private int nextObjectForCreateIndex
    {
        get
        {
            ++this.objectForCreateIndex;
            if (this.objectForCreateIndex >= this.objectsForCreate.Count)
            {
                this.objectForCreateIndex = 0;
            }
            return this.objectForCreateIndex;
        }
    }
    protected List<GameObject> objectsForCreate = new List<GameObject>();
    private GameObject nextObjectForCreate
    {
        get => this.objectsForCreate[this.nextObjectForCreateIndex];
    }
    protected virtual void SetObjectsForCreate()
    {
        this.objectsForCreate.Add(this.objectPrefab);
    }
    private void SetRandomRotation(GameObject go)
    {
        Transform transform = go.transform;
        // Generate random Euler angles for rotation
        float randomY = GetRandomNumberByRange(0f, 360f);

        //Задать вращение только вокруг себя по вертикали
        Quaternion randomRotation = Quaternion.Euler(0, randomY, 0);

        // Set the random rotation for the GameObject
        transform.rotation = randomRotation;
    }
    void Start()
    {
        if (objectPrefab == null || boundsTransform == null)
        {
            Debug.LogError("ObjectPrefab or BoundsTransform is not assigned!");
            return;
        }
        this.random = new System.Random(this.seed);

        SetObjectsForCreate();
        Vector2 objectHalfSize = new Vector2
            (
            this.objectPrefab.transform.localScale.x / 2,
            this.objectPrefab.transform.localScale.z / 2
            );
        // Get the bounds of the transform
        Bounds bounds = boundsTransform.GetComponent<Renderer>().bounds;

        // Calculate the total area within the bounds
        float totalArea = bounds.size.x * bounds.size.z;

        // Calculate the number of objects based on the desired density
        int objectCount = Mathf.RoundToInt(totalArea * density);

        for (int i = 0; i < objectCount; i++)
        {
            // Generate a random position within the bounds
            Vector3 randomPosition = new Vector3
                (
                GetRandomNumberByRange(bounds.min.x+objectHalfSize.x, bounds.max.x-objectHalfSize.x),
                bounds.center.y,
                GetRandomNumberByRange(bounds.min.z+objectHalfSize.y, bounds.max.z-objectHalfSize.y)
                );

            // Instantiate the object at the random position
            GameObject newObj = Instantiate(this.nextObjectForCreate, randomPosition, Quaternion.identity);
            this.createdObjects.Add(newObj);
            SetRandomRotation(newObj);
            newObj.transform.SetParent(this.boundsTransform);
        }

        Debug.Log($"Objects created: {this.createdObjects.Count}");
    }
}
