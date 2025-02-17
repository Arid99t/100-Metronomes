using UnityEngine;
using System.Collections.Generic;

public class MetronomeLightController : MonoBehaviour
{
    [SerializeField]
    private float syncThreshold = 70f;
    private SyncCalculatorIndividual syncCalculator;
    private Dictionary<GameObject, Light> metronomeLights = new Dictionary<GameObject, Light>();

    private void Start()
    {
        syncCalculator = GetComponent<SyncCalculatorIndividual>();
        if (syncCalculator == null)
        {
            Debug.LogError("SyncCalculatorIndividual not found!");
            return;
        }

        // Find all metronomes
        GameObject[] metronomes = GameObject.FindGameObjectsWithTag("Metro");
        Debug.Log($"Found {metronomes.Length} objects with Metro tag");

        int totalLightsFound = 0;
        foreach (GameObject metro in metronomes)
        {
            // Get ALL lights in the hierarchy, not just immediate children
            Light[] lights = metro.GetComponentsInChildren<Light>(true);

            if (lights.Length > 0)
            {
                // Use the first light found
                metronomeLights.Add(metro, lights[0]);
                lights[0].enabled = false;
                totalLightsFound++;
                Debug.Log($"Found light for metronome {metro.name} at path: {GetGameObjectPath(lights[0].gameObject)}");
            }
            else
            {
                Debug.LogWarning($"No light found in hierarchy of metronome {metro.name}!");
                // Print the full hierarchy to help debugging
                PrintHierarchy(metro.transform, "");
            }
        }

        Debug.Log($"Total lights found and mapped: {totalLightsFound}");
    }

    private void Update()
    {
        if (syncCalculator == null) return;

        var metronomesField = typeof(SyncCalculatorIndividual).GetField("metronomes",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (metronomesField == null)
        {
            Debug.LogError("Failed to access metronomes field!");
            return;
        }

        var metronomeDataList = metronomesField.GetValue(syncCalculator) as System.Collections.IList;
        if (metronomeDataList == null || metronomeDataList.Count == 0) return;

        foreach (var metronomeData in metronomeDataList)
        {
            if (metronomeData == null) continue;

            var metroObjectField = metronomeData.GetType().GetField("metroObject");
            var syncValueField = metronomeData.GetType().GetField("individualSync");

            if (metroObjectField == null || syncValueField == null) continue;

            GameObject metroObject = metroObjectField.GetValue(metronomeData) as GameObject;
            float syncValue = (float)syncValueField.GetValue(metronomeData);

            if (metronomeLights.TryGetValue(metroObject, out Light light))
            {
                // Update light state based on sync value
                bool shouldBeEnabled = syncValue >= syncThreshold;
                if (light.enabled != shouldBeEnabled)
                {
                    light.enabled = shouldBeEnabled;
                    if (shouldBeEnabled)
                    {
                        Debug.Log($"Enabling light for {metroObject.name} at sync {syncValue:F2}%");
                    }
                }
            }
        }
    }

    // Helper method to get the full path of a GameObject in hierarchy
    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }

    // Helper method to print the full hierarchy of an object
    private void PrintHierarchy(Transform transform, string indent)
    {
        Debug.Log($"{indent}GameObject: {transform.name}");
        Debug.Log($"{indent}Components: {string.Join(", ", GetComponentNames(transform))}");

        foreach (Transform child in transform)
        {
            PrintHierarchy(child, indent + "  ");
        }
    }

    private string[] GetComponentNames(Transform transform)
    {
        Component[] components = transform.GetComponents<Component>();
        string[] names = new string[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            names[i] = components[i].GetType().Name;
        }
        return names;
    }
}