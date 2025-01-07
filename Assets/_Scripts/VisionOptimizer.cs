using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionOptimizer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Arrastra aquí la Main Camera
    [SerializeField] private float defaultMargin = 10f; // Margen por defecto para los Terrains
    [SerializeField] private float maxDistance = 60f; // Límite de distancia

    private Plane[] frustumPlanes; // Define los límites del cono de visión
    private Dictionary<Terrain, float> marginLookup = new Dictionary<Terrain, float>();

    void Start()
{
    // Referencia al objeto Forest
    Transform forestTransform = GameObject.Find("Forest").transform;
    if (forestTransform == null)
    {
        Debug.LogError("No se encontró el objeto Forest. Verifica la jerarquía.");
        return;
    }

    // Asigna márgenes específicos para cada Terrain
    marginLookup[GameObject.Find("Garden").GetComponent<Terrain>()] = 10f;
    marginLookup[GameObject.Find("Ruins").GetComponent<Terrain>()] = 20f;
    marginLookup[GameObject.Find("Graveyard").GetComponent<Terrain>()] = 20f;
    marginLookup[GameObject.Find("Volcano").GetComponent<Terrain>()] = 50f;

    // Recorre los hijos de Forest para asignar los márgenes
    foreach (Transform child in forestTransform)
    {
        var terrain = child.GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogWarning($"Ignorando el hijo {child.name} de Forest porque no tiene componente Terrain.");
            continue; // Salta este objeto
        }

        marginLookup[terrain] = 35f;
        Debug.Log($"Margen asignado correctamente: {child.name} con margen 35f");
    }
}

    void Update()
    {
        // Calcula los planos del frustum de la cámara
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Procesar los Terrains
        ProcessTerrains();

        // Procesar otros objetos del escenario
        ProcessObjects();
    }

    private void ProcessTerrains()
    {
        // Encuentra todos los Terrains en la escena
        Terrain[] terrains = FindObjectsOfType<Terrain>();

        foreach (Terrain terrain in terrains)
        {
            // Verifica si el Terrain está activo y tiene datos válidos
            if (terrain == null || terrain.terrainData == null)
                continue;

            // Obtén el margen específico o usa el margen por defecto
            float margin = marginLookup.ContainsKey(terrain) ? marginLookup[terrain] : defaultMargin;

            // Calcula los Bounds del Terrain
            Bounds terrainBounds = terrain.terrainData.bounds;
            terrainBounds.center += terrain.GetPosition();
            terrainBounds.Expand(margin); // Aplica el margen al Bounds

            // Comprueba si el Terrain está en el frustum y dentro de la distancia máxima (con margen)
            bool isInFrustum = GeometryUtility.TestPlanesAABB(frustumPlanes, terrainBounds);
            bool isWithinDistance = Vector3.Distance(mainCamera.transform.position, terrainBounds.center) <= maxDistance + margin;

            // Renderiza o desactiva el Terrain según las condiciones
            terrain.drawHeightmap = isInFrustum && isWithinDistance;
        }
    }

    private void ProcessObjects()
    {
        // Encuentra todos los objetos en la escena
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Ignorar objetos sin padres activos o sin transformaciones válidas
            if (renderer == null || renderer.transform == null || !renderer.gameObject.activeInHierarchy)
                continue;

            // Excluir la Red Moon
            if (renderer.gameObject.name == "Red Moon")
            {
                renderer.enabled = true; // Asegurarse de que siempre está activa
                continue;
            }

            // Calcula los Bounds del objeto
            Bounds objectBounds = renderer.bounds;

            // Comprueba si el objeto está en el frustum y dentro de la distancia máxima
            bool isInFrustum = GeometryUtility.TestPlanesAABB(frustumPlanes, objectBounds);
            bool isWithinDistance = Vector3.Distance(mainCamera.transform.position, objectBounds.center) <= maxDistance;

            // Activa o desactiva el renderizado según las condiciones
            renderer.enabled = isInFrustum && isWithinDistance;

            // Opcional: Desactiva el GameObject si no está visible
            // renderer.gameObject.SetActive(isInFrustum && isWithinDistance);
        }
    }
}