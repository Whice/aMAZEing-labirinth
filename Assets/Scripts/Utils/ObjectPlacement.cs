using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    /// <summary>
    /// Скрипт для размещения объектов в указанных границах.
    /// </summary>
    public class ObjectPlacement : MonoBehaviourLogger
    {
        /// <summary>
        /// Объекты для размещения.
        /// </summary>
        [SerializeField] protected GameObject[] objectPrefabs = new GameObject[0];
        /// <summary>
        /// Границы, в которых надо рамещать объекты.
        /// </summary>
        [SerializeField] private Transform boundsTransform;
        /// <summary>
        /// Плотсность размещения.
        /// </summary>
        [SerializeField] private float density = 1f;
        /// <summary>
        /// Смещение от границ.
        /// </summary>
        [SerializeField] private float borderOffset = 0.25f;
        /// <summary>
        /// Созданные объекты.
        /// </summary>
        private List<GameObject> createdObjects = new List<GameObject>();

        /// <summary>
        /// Сид для рандома, который используется для расчета положений в мире.
        /// </summary>
        public int seed = 0;
        private System.Random random;

        /// <summary>
        /// Случайное число между заданными.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected float GetRandomNumberByRange(float min, float max)
        {
            return (float)this.random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Получить объект по заданному индексу.
        /// Индексы зациклены, 
        /// если длина массива 4, а индекс 5, то конецный индекс будет 1.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private GameObject GetObjectByIndex(int index)
        {
            int indexInArray = index % this.objectPrefabs.Length;
            return this.objectPrefabs[indexInArray];
        }
        /// <summary>
        /// Задать случайное врещение вокруг вертикальной оси.
        /// </summary>
        /// <param name="go"></param>
        private void SetRandomRotation(GameObject go)
        {
            Transform transform = go.transform;
            // Generate random Euler angles for rotation
            float randomY = GetRandomNumberByRange(0f, 360f);

            // Задать вращение только вокруг себя по вертикали
            Quaternion randomRotation = Quaternion.Euler(0, randomY, 0);

            // Set the random rotation for the GameObject
            transform.rotation = randomRotation;
        }
        /// <summary>
        /// Проверить пересечение с прочими объектами.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CheckOverlap(GameObject obj)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
                return false;

            foreach (GameObject createdObj in createdObjects)
            {
                Collider createdObjCollider = createdObj.GetComponent<Collider>();
                if (createdObjCollider != null && createdObjCollider.bounds.Intersects(objCollider.bounds))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Скрипт для объединения мешей с одинаковым материалом в один меш.
        /// </summary>
        private MeshCombiner meshCombiner = new MeshCombiner(true);

        /// <summary>
        /// Разместить объекты.
        /// </summary>
        public void PlaceObjects()
        {
            if (this.objectPrefabs == null || this.objectPrefabs.Length == 0 || this.boundsTransform == null)
            {
                Debug.LogError("ObjectPrefab or BoundsTransform is not assigned!");
                return;
            }

            this.random = new System.Random(this.seed);

            // Get the bounds of the transform
            Bounds bounds = this.boundsTransform.GetComponent<Renderer>().bounds;

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

                    Vector2 boundShift = this.borderOffset * new Vector2(bounds.extents.x, bounds.extents.z);

                    bool isOverlapping = true;
                    int maxAttempts = 10;
                    int attemptCount = 0;

                    // Generate a random position within the bounds
                    while (isOverlapping && attemptCount < maxAttempts)
                    {
                        Vector3 randomPosition = new Vector3
                        (
                            GetRandomNumberByRange(bounds.min.x + boundShift.x, bounds.max.x - boundShift.x),
                            bounds.center.y,
                            GetRandomNumberByRange(bounds.min.z + boundShift.y, bounds.max.z - boundShift.y)
                        );

                        // Instantiate the object at the random position
                        GameObject newObj = Instantiate(nextObjectForCreate, randomPosition, Quaternion.identity);

                        if (!CheckOverlap(newObj))
                        {
                            this.createdObjects.Add(newObj);
                            SetRandomRotation(newObj);
                            newObj.transform.SetParent(this.boundsTransform);
                            isOverlapping = false;
                        }
                        else
                        {
                            Destroy(newObj);
                            attemptCount++;
                        }
                    }
                }
            }

            this.meshCombiner.SetObjects(this.createdObjects);
            this.meshCombiner.CombineMeshes(this.boundsTransform);
        }
        void Start()
        {
            PlaceObjects();
        }
    }
}