using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionOptimizer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Arrastra aquí la Main Camera
    [SerializeField] private float maxDistance = 51f; // Límite de distancia

    private Plane[] frustumPlanes; // Define los límites del cono de visión

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

            // Calcula los Bounds del Terrain
            Bounds terrainBounds = terrain.terrainData.bounds;
            terrainBounds.center += terrain.GetPosition();

            // Comprueba si el Terrain está en el frustum y dentro de la distancia máxima
            bool isInFrustum = GeometryUtility.TestPlanesAABB(frustumPlanes, terrainBounds);
            bool isWithinDistance = Vector3.Distance(mainCamera.transform.position, terrainBounds.center) <= maxDistance;

            // Renderiza o desactiva el Terrain según las condiciones
            if (isInFrustum && isWithinDistance)
            {
                terrain.drawHeightmap = true; // Activa el renderizado del Terrain
                Debug.Log($"Renderizando Terrain: {terrain.name}");
            }
            else
            {
                terrain.drawHeightmap = false; // Desactiva el renderizado del Terrain
                Debug.Log($"Ocultando Terrain: {terrain.name}");
            }
        }
    }
}