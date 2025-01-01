using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionOptimizer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Arrastra aquí la Main Camera
    [SerializeField] private float defaultMargin = 10f; // Margen por defecto para los Terrains
    [SerializeField] private float maxDistance = 51f; // Límite de distancia

    private Plane[] frustumPlanes; // Define los límites del cono de visión
    private Dictionary<Terrain, float> marginLookup = new Dictionary<Terrain, float>();

    void Start()
    {
        // Asigna márgenes específicos para cada Terrain
        marginLookup[GameObject.Find("Garden").GetComponent<Terrain>()] = 10f;
        marginLookup[GameObject.Find("Ruins").GetComponent<Terrain>()] = 20f;
        marginLookup[GameObject.Find("Graveyard").GetComponent<Terrain>()] = 20f;
        marginLookup[GameObject.Find("Forest").GetComponent<Terrain>()] = 35f;
        marginLookup[GameObject.Find("Volcano").GetComponent<Terrain>()] = 50f;
    }

    void Update()
    {
        // Calcula los planos del frustum de la cámara
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

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
            if (isInFrustum && isWithinDistance)
            {
                terrain.drawHeightmap = true; // Activa el renderizado del Terrain
                Debug.Log($"Renderizando Terrain: {terrain.name} con margen {margin}");
            }
            else
            {
                terrain.drawHeightmap = false; // Desactiva el renderizado del Terrain
                Debug.Log($"Ocultando Terrain: {terrain.name}");
            }
        }
    }
}