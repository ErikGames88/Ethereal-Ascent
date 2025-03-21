using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private Renderer waterRenderer;
    private float speedX = 0.03f;
    private float speedY = 0.03f;

    void Update()
    {
        float offsetX =  speedX * Time.time;
        float offsetY =  speedY * Time.time;
        waterRenderer.material.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));
    }
}