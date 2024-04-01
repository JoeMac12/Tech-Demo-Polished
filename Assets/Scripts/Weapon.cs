using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int clipSize = 30;
    public float fireDelay = 0.2f;
    public float reloadSpeed = 2f;
    public AudioSource fireSound;
    public AudioClip clip;
    public AudioClip reloadSound;

    public GameObject muzzleFlashObject;
    public Camera fpsCam;
    public TextMeshProUGUI ammoText;

    public Transform gunTransform;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 8f;
    private Vector3 originalPosition;

    private Quaternion originalRotation;

    public float reloadRotationAmount = 20f;
    public float reloadRotationSpeed = 4f;

    public Transform firePoint;
    public TrailRenderer bulletTrail;
    public float bulletForce = 20f;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = clipSize;
        fireSound = GetComponent<AudioSource>();
        originalPosition = gunTransform.localPosition;
        originalRotation = gunTransform.localRotation;

        UpdateAmmoText();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && !isReloading)
        {
            if (Time.time > nextFireTime && currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + fireDelay;
                currentAmmo--;
                UpdateAmmoText();
            }
            else if (currentAmmo == 0)
            {
                Reload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }

        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
    }

    void Shoot()
    {
        fireSound.PlayOneShot(clip);

        var bullet = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
        bullet.AddPosition(firePoint.position);
        {
            bullet.transform.position = transform.position + (fpsCam.transform.forward * 200);
        }

        if (muzzleFlashObject != null)
        {
            muzzleFlashObject.SetActive(true);
            Invoke("DisableMuzzleFlash", 0.05f);
        }

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
        {
            Debug.Log("Hit: " + hit.transform.name);
        }

        gunTransform.Translate(0, 0, -recoilDistance);
    }

    void Reload()
    {
        if (currentAmmo < clipSize)
        {
            if (reloadSound != null) {
                fireSound.PlayOneShot(reloadSound);
            }

            isReloading = true;
            Debug.Log("Reloading...");

            Invoke("FinishReload", reloadSpeed);
            StartCoroutine(RotateGun());
        }
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }

    void FinishReload()
    {
        currentAmmo = clipSize;
        StartCoroutine(ResetGun());
        UpdateAmmoText();
    }

    void DisableMuzzleFlash()
    {
        muzzleFlashObject.SetActive(false);
    }

    // Losing my mind rn

    IEnumerator RotateGun() {
        isReloading = true;
        Debug.Log("Reloading...");

        Quaternion targetRotation = Quaternion.Euler(reloadRotationAmount, 0, 0);

        while (gunTransform.localRotation != targetRotation) {
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, targetRotation, Time.deltaTime * reloadRotationSpeed);
            yield return null;
        }
    }

    IEnumerator ResetGun() {
        while (gunTransform.localPosition != originalPosition || gunTransform.localRotation != originalRotation) {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, originalRotation, Time.deltaTime * reloadRotationSpeed);
            yield return null;
        }

        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
