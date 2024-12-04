using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab de la bala")]
    private GameObject bulletPrefab;

    [SerializeField, Tooltip("Punto de origen del disparo")]
    private Transform firePoint;

    [SerializeField, Tooltip("Velocidad de la bala")]
    private float bulletSpeed = 20f;

    [SerializeField, Tooltip("Distancia máxima de la bala antes de destruirse")]
    private float maxBulletDistance = 50f;

    [SerializeField, Tooltip("Efecto de fuego del disparo")]
    private ParticleSystem fireShot;

    [SerializeField, Tooltip("Sonido del disparo")]
    private AudioClip fireSound;

    [SerializeField, Tooltip("Sonido de recarga")]
    private AudioClip reloadSound;

    private AudioSource audioSource;

    [SerializeField, Tooltip("Máximo de balas por recarga")]
    private int maxAmmo = 9;

    private int currentAmmo;

    private bool canShoot = true;
    private bool isReloading = false;

    private CrosshairManager crosshairManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        crosshairManager = FindObjectOfType<CrosshairManager>();
    }

    void Start()
    {
        // Inicializar con el máximo de balas
        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isReloading) // Botón derecho del mouse
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading) // Recargar con la tecla R
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (!canShoot || currentAmmo <= 0)
        {
            Debug.LogWarning("No puedes disparar todavía o no tienes balas.");
            return;
        }

        StartCoroutine(HandleShotRate());

        currentAmmo--;

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                Vector3 targetPoint;
                if (crosshairManager != null)
                {
                    targetPoint = crosshairManager.GetCrosshairWorldPosition();
                }
                else
                {
                    targetPoint = firePoint.position + firePoint.forward * 10f;
                }

                Vector3 direction = (targetPoint - firePoint.position).normalized;
                bulletRb.velocity = direction * bulletSpeed;
            }

            Destroy(bullet, maxBulletDistance / bulletSpeed);
        }

        if (fireShot != null)
        {
            fireShot.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            fireShot.Play();
        }

        // Reproducir sonido del disparo
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
            Debug.Log("Sonido de disparo reproducido.");
        }
        else
        {
            Debug.LogWarning("No se ha asignado el AudioSource o el AudioClip para el disparo.");
        }

        Debug.Log($"Disparo realizado. Balas restantes: {currentAmmo}");
    }

    private IEnumerator HandleShotRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    private void Reload()
    {
        if (currentAmmo == maxAmmo)
        {
            Debug.LogWarning("El cargador ya está lleno.");
            return;
        }

        StartCoroutine(HandleReload());
    }

    private IEnumerator HandleReload()
    {
        isReloading = true;

        // Reproducir sonido de recarga
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el AudioSource o el AudioClip para la recarga.");
        }

        Debug.Log("Recargando...");

        yield return new WaitForSeconds(2f); // Tiempo de recarga

        currentAmmo = maxAmmo; // Recargar completamente
        isReloading = false;

        Debug.Log("Revólver recargado.");
    }
}
