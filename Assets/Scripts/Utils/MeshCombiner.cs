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
        /// Надо ли уничтожать все переданные объекты.
        /// </summary>
        private bool isDestroyAllOldObjects;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDestroyAllOldObjects">Надо ли уничтожать все переданные объекты.</param>
        public MeshCombiner(bool isDestroyAllOldObjects = false)
        {
            this.isDestroyAllOldObjects = isDestroyAllOldObjects;
        }

        /// <summary>
        /// Объекты отрисовки для объеденения
        /// </summary>
        private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        /// <summary>
        /// Список объектов, из которых получены объекты отрисовки.
        /// </summary>
        private List<GameObject> oldGameObjects = new List<GameObject>();
        /// <summary>
        /// Задать список объектов для объеденения.
        /// </summary>
        /// <param name="gameObjects"></param>
        public void SetObjects(List<GameObject> gameObjects)
        {
            this.oldGameObjects.Clear();
            this.oldGameObjects.AddRange(gameObjects);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                // Get all MeshRenderer components from children objects
                MeshRenderer[] meshRenderers = gameObjects[i].GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer meshRenderer in meshRenderers)
                    if (meshRenderer != null)
                    {
                        this.meshRenderers.Add(meshRenderer);
                    }
            }
        }
        /// <summary>
        /// Объеденить в один меш все прочие, у которых один материал.
        /// </summary>
        /// <param name="parent">Родительский узел для новых мешей.</param>
        public void CombineMeshes(Transform parent)
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
                        if (!this.isDestroyAllOldObjects)
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
                    }
                }
            }

            //Уничтожить старые объекты, если требуется.
            if (this.isDestroyAllOldObjects)
            {
                //ToDo надо не уничтожать те объекты, которые не были затронуты объеденениями.
                GameObject lastGO = null;
                while (this.oldGameObjects.Count != 0)
                {
                    lastGO = this.oldGameObjects[this.oldGameObjects.Count - 1];
                    GameObject.Destroy(lastGO);
                    this.oldGameObjects.RemoveAt(this.oldGameObjects.Count - 1);
                }
            }
        }
    }
}