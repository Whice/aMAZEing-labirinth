using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] protected GameObject[] objectPrefabs = new GameObject[0];
    [SerializeField] private Transform boundsTransform;
    [SerializeField] private float density = 1f;
    private List<GameObject> createdObjects= new List<GameObject>();

    public int seed = 0;
    private System.Random random;
    protected float GetRandomNumberByRange(float min, float max)
    {
        return (float)this.random.NextDouble() * (max - min) + min;
    }

    private GameObject GetObjectByIndex(int index)
    {
        int indexInArray = index%this.objectPrefabs.Length;
        return this.objectPrefabs[indexInArray];
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
        if (objectPrefabs == null || boundsTransform == null)
        {
            Debug.LogError("ObjectPrefab or BoundsTransform is not assigned!");
            return;
        }
        this.random = new System.Random(this.seed);

        // Get the bounds of the transform
        Bounds bounds = boundsTransform.GetComponent<Renderer>().bounds;

        // Calculate the total area within the bounds
        float totalArea = bounds.size.x * bounds.size.z;

        // Calculate the number of objects based on the desired density
        int objectCount = Mathf.RoundToInt(totalArea * density);

        int typesCount = objectCount / this.objectPrefabs.Length;
        for (int objIndex = 0; objIndex < this.objectPrefabs.Length; objIndex++)
        {
            int start = objIndex * typesCount;
            int end = (objIndex + 1) * typesCount;
            for (int i = start; i < end; i++)
            {
                GameObject nextObjectForCreate = GetObjectByIndex(objIndex);

                Vector2 boundShift = 0.25f * new Vector2(bounds.extents.x, bounds.extents.z);
                // Generate a random position within the bounds
                Vector3 randomPosition = new Vector3
                    (
                    GetRandomNumberByRange(bounds.min.x+ boundShift.x, bounds.max.x - boundShift.x),
                    bounds.center.y,
                    GetRandomNumberByRange(bounds.min.z + boundShift.y, bounds.max.z -boundShift.y)
                    );
                // Instantiate the object at the random position
                GameObject newObj = Instantiate(nextObjectForCreate, randomPosition, Quaternion.identity);
                this.createdObjects.Add(newObj);
                SetRandomRotation(newObj);
                newObj.transform.SetParent(this.boundsTransform);
            }
        }

        Debug.Log($"Objects created: {this.createdObjects.Count}");
    }
}
