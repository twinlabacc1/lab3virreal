using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject berserkPrefab;
    [SerializeField] private GameObject malePrefab;
    [SerializeField] private GameObject femalePrefab;

    [SerializeField] private Vector3 berserkPrefabOffset;
    [SerializeField] private Vector3 malePrefabOffset;
    [SerializeField] private Vector3 femalePrefabOffset;

    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<TrackableId, List<GameObject>> spawnedObjects = new(); 

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        }
    }

    private void OnDisable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
        }
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added)
        {
            CreatePrefabs(image);
        }

        foreach (ARTrackedImage image in eventArgs.updated)
        {
            UpdatePrefabsPosition(image);
        }
    }

    private void CreatePrefabs(ARTrackedImage image)
    {
        if (!spawnedObjects.ContainsKey(image.trackableId))
        {
            spawnedObjects[image.trackableId] = new List<GameObject>();
        }

        // Створюємо Berserk
        GameObject berserk = Instantiate(berserkPrefab, image.transform.position + berserkPrefabOffset, image.transform.rotation);
        berserk.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(berserk);

        // Створюємо Male
        GameObject male = Instantiate(malePrefab, image.transform.position + malePrefabOffset, image.transform.rotation);
        male.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(male);

        // Створюємо Female
        GameObject female = Instantiate(femalePrefab, image.transform.position + femalePrefabOffset, image.transform.rotation);
        female.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(female);
    }

    private void UpdatePrefabsPosition(ARTrackedImage image)
    {
        if (spawnedObjects.TryGetValue(image.trackableId, out List<GameObject> existingObjects))
        {
            // Оновлюємо позицію та ротацію для Berserk
            if (existingObjects.Count > 0 && existingObjects[0] != null)
            {
                existingObjects[0].transform.position = image.transform.position + berserkPrefabOffset;
                existingObjects[0].transform.rotation = image.transform.rotation;
            }

            // Оновлюємо позицію та ротацію для Male
            if (existingObjects.Count > 1 && existingObjects[1] != null)
            {
                existingObjects[1].transform.position = image.transform.position + malePrefabOffset;
                existingObjects[1].transform.rotation = image.transform.rotation;
            }

            // Оновлюємо позицію та ротацію для Female
            if (existingObjects.Count > 2 && existingObjects[2] != null)
            {
                existingObjects[2].transform.position = image.transform.position + femalePrefabOffset;
                existingObjects[2].transform.rotation = image.transform.rotation;
            }
        }
    }
}
