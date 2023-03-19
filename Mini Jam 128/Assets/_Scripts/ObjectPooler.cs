using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsTest
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Pool Variables")]
        [SerializeField] private GameObject _prefab;            // The GameObject prefab to populate the pool
        [SerializeField] private int _poolSize;                 // The size of the object pool
        [SerializeField] private bool _isExpandable;            // Check if the pool can be expanded if the number of objects reaches the maximum size

        private List<GameObject> _availableObjectsList;         // The list of objects that are available to be called
        private List<GameObject> _employedObjectsList;          // The list of objects that are currently being used and unavailable

        private void Awake()
        {
            _availableObjectsList = new List<GameObject>();     // Create a new available GameObjects list
            _employedObjectsList = new List<GameObject>();      // Create a new employed GameObjects list

            for (int i = 0; i < _poolSize; i++)
            {
                GenerateNewObject();                            // Fill the pool with the respective object quantities
            }
        }

        private void GenerateNewObject()                        // Instantiate a new GameObject to add to the list
        {
            GameObject obj = Instantiate(_prefab);
            obj.transform.parent = transform;
            obj.SetActive(false);
            _availableObjectsList.Add(obj);
        }

        public GameObject GetObject()                           // Get a GameObject from the available objects pool and add it to the used pool
        {
            int totalObjectsAvailable = _availableObjectsList.Count;

            if (totalObjectsAvailable == 0 && !_isExpandable) return null;
            else if (totalObjectsAvailable == 0) GenerateNewObject();

            GameObject obj = _availableObjectsList[totalObjectsAvailable - 1];
            _availableObjectsList.RemoveAt(totalObjectsAvailable - 1);
            _employedObjectsList.Add(obj);

            return obj;
        }

        public void ReturnObject(GameObject obj)                // Return a GameObject to the available objects pool
        {
            Debug.Assert(_employedObjectsList.Contains(obj));   // Verify if the used list actually contains the GameObject
            obj.SetActive(false);
            _employedObjectsList.Remove(obj);
            _availableObjectsList.Add(obj);
        }
    }
}