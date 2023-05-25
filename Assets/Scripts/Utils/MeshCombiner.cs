using System;
using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    /// <summary>
    /// Скрипт для объеденения всех мешей с одинковыми материалами в один.
    /// <br/>Напоминание: для всех объединяемых мешей, 
    /// которые были импортированы,
    /// надо установить флаг ReadWrite в настройках (у юнитевских мешей он есть по умолчанию).
    /// </summary>
    public class MeshCombiner
    {
        /// <summary>
        /// Надо ли уничтожать все объедененные объекты.
        /// </summary>
        private bool isDestroyAllOldObjects;
        /// <summary>
        /// Надо ли уничтожать все дочерние объекты.
        /// </summary>
        private bool isDestroyAllChildrenObjects;
        /// <summary>
        /// Если для уничтожения объектов установлены одинаковые флаги, то они не будут уничтожены.
        /// Даже, если оба флага - true.
        /// </summary>
        /// <param name="isDestroyAllOldObjects">Надо ли уничтожать все переданные объекты.</param>
        /// <param name="isDestroyAllChildrenObjects">Надо ли уничтожать дочерние объекты.</param>
        public MeshCombiner(bool isDestroyAllOldObjects = false, bool isDestroyAllChildrenObjects = false)
        {
            this.isDestroyAllOldObjects = isDestroyAllOldObjects;
            this.isDestroyAllChildrenObjects = isDestroyAllChildrenObjects;
        }

        /// <summary>
        /// Список объектов, из которых получены объекты отрисовки.
        /// </summary>
        private List<GameObject> oldGameObjects = new List<GameObject>();
        /// <summary>
        /// Запомнить список дочерних объектов.
        /// </summary>
        private void RememberChildGameobjects(Transform parent)
        {
            this.oldGameObjects.Clear();

            int childrenCount = parent.childCount;

            for (int i = 0; i < childrenCount; i++)
            {
                this.oldGameObjects.Add(parent.GetChild(i).gameObject);
            }
        }
        /// <summary>
        /// Объекты созданные при объединении мешей.
        /// </summary>
        public List<GameObject> newObjects { get; private set; } = new List<GameObject>();
        /// <summary>
        /// Объеденить в один меш все прочие, у которых один материал.
        /// </summary>
        /// <param name="parent">Родительский узел для новых мешей.</param>
        public void CombineMeshes(Transform parent, MeshRenderer[] meshRenderers)
        {
            Dictionary<Material, List<MeshFilter>> meshesByMaterial = new Dictionary<Material, List<MeshFilter>>();

            // Группируем меши по материалу
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Material material = meshRenderer.sharedMaterial;
                MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();

                if (material != null && meshFilter != null)
                {
                    if (!meshesByMaterial.ContainsKey(material))
                    {
                        meshesByMaterial[material] = new List<MeshFilter>();
                    }

                    meshesByMaterial[material].Add(meshFilter);
                }
            }

            // Объединяем меши с одинаковыми материалами
            foreach (var kvp in meshesByMaterial)
            {
                Material material = kvp.Key;
                List<MeshFilter> meshes = kvp.Value;

                if (meshes.Count > 1)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    int vertexCounts = 0;
                    const int MAX_VERTEX_COUNT = (int)(Int16.MaxValue * 0.9f);
                    List<List<CombineInstance>> combineInstancesLists = new List<List<CombineInstance>>();
                    combineInstancesLists.Add(new List<CombineInstance>());
                    int combineInstancesListsLastIndex = 0;
                    for (int i = 0; i < meshes.Count; i++)
                    {
                        combineInstance.mesh = meshes[i].sharedMesh;
                        combineInstance.transform = meshes[i].transform.localToWorldMatrix;

                        vertexCounts += combineInstance.mesh.vertexCount;
                        //Заранее учесть выход за пределы максимального количества вершин для объеденения
                        if (vertexCounts > MAX_VERTEX_COUNT)
                        {
                            combineInstancesLists.Add(new List<CombineInstance>());
                            vertexCounts = 0;
                            ++combineInstancesListsLastIndex;
                        }

                        combineInstancesLists[combineInstancesListsLastIndex].Add(combineInstance);

                        //Незачем отдельно унитожать, если будут оничтожены все переданные объекты
                        if (!this.isDestroyAllChildrenObjects && this.isDestroyAllOldObjects)
                            GameObject.Destroy(meshes[i].gameObject);
                    }

                    foreach (List<CombineInstance> combineInstances in combineInstancesLists)
                    {
                        GameObject combinedObject = new GameObject("CombinedObject");
                        combinedObject.transform.SetParent(parent);

                        MeshFilter meshFilterCombined = combinedObject.AddComponent<MeshFilter>();
                        meshFilterCombined.mesh = new Mesh();
                        meshFilterCombined.mesh.CombineMeshes(combineInstances.ToArray());

                        MeshRenderer meshRendererCombined = combinedObject.AddComponent<MeshRenderer>();
                        meshRendererCombined.sharedMaterial = material;
                        this.newObjects.Add(combinedObject);
                    }
                }
            }

            //Уничтожить старые объекты, если требуется.
            if (this.isDestroyAllChildrenObjects && !this.isDestroyAllOldObjects)
            {
                for (int i = 0; i < this.oldGameObjects.Count; i++)
                {
                    GameObject.Destroy(this.oldGameObjects[i]);
                }
                this.oldGameObjects.Clear();
            }
        }
        /// <summary>
        /// Объеденить в один меш все прочие, у которых один материал.
        /// Меши с одинаковыми материалами будут искаться среди дочерних элементов.
        /// </summary>
        /// <param name="parent">Родительский узел для новых мешей.</param>
        public void CombineMeshes(Transform parent)
        {
            this.newObjects.Clear();

            if (this.isDestroyAllChildrenObjects)
                RememberChildGameobjects(parent);

            MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

            List<MeshRenderer> meshRenderersList = new List<MeshRenderer>(meshRenderers.Length);
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                if (meshRenderers != null)
                {
                    meshRenderersList.Add(meshRenderer);
                }
            }

            CombineMeshes(parent, meshRenderers);
        }
        /// <summary>
        /// Объеденить в один меш все прочие, у которых один материал.
        /// Объединит меши с одинаковыми материалами принадлежащие объектам и их дочерним объектам,
        /// и добавить новые объекты дочерними к указанному родителю.
        /// </summary>
        /// <param name="parent">Родительский узел для новых мешей.</param>
        public void CombineMeshes(Transform parent, List<GameObject> gameObjects)
        {
            this.oldGameObjects.Clear();
            this.oldGameObjects.AddRange(gameObjects);
            HashSet<MeshRenderer> meshRenderersHashSet = new HashSet<MeshRenderer>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                MeshRenderer objectMeshRenderer = gameObjects[i].GetComponent<MeshRenderer>();
                if (objectMeshRenderer != null)
                    meshRenderersHashSet.Add(objectMeshRenderer);

                MeshRenderer[] meshRenderers = gameObjects[i].GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in meshRenderers)
                    if (meshRenderer != null)
                        meshRenderersHashSet.Add(meshRenderer);
            }

            MeshRenderer[] meshRenderersArray = new MeshRenderer[meshRenderersHashSet.Count];
            int counter = 0;
            foreach (MeshRenderer meshRenderer in meshRenderersHashSet)
            {
                meshRenderersArray[counter] = meshRenderer;
                ++counter;
            }

            CombineMeshes(parent, meshRenderersArray);
        }
    }
}