using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private HandGrabInteractor primaryHandGrab;
    [SerializeField] private HandGrabInteractor secondaryHandGrab;
    
    private bool isGrabbed;
    private bool isTwoHanded;
    private bool canShoot = true;
    private float shootCooldown = 0.1f;
      private void Start()
      {
          if (primaryHandGrab != null)
          {
              primaryHandGrab.WhenSelectingInteractableChanged += (interactable) =>
              {
                  if (interactable != null)
                      OnPrimaryGrabbed();
                  else
                      OnPrimaryReleased();
              };
          }

          if (secondaryHandGrab != null)
          {
              secondaryHandGrab.WhenSelectingInteractableChanged += (interactable) =>
              {
                  if (interactable != null)
                      OnSecondaryGrabbed();
                  else
                      OnSecondaryReleased();
              };
          }
      }
    }

    private void Update()
    {
        if (isGrabbed && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && 
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Shoot();
        }
    }

    private void OnPrimaryGrabbed()
    {
        isGrabbed = true;
        UpdateGripState();
    }

    private void OnPrimaryReleased()
    {
        isGrabbed = false;
        UpdateGripState();
    }

    private void OnSecondaryGrabbed() => UpdateGripState();
    private void OnSecondaryReleased() => UpdateGripState();

    private void UpdateGripState()
    {
        isTwoHanded = primaryHandGrab.HasSelectedInteractable && 
                      secondaryHandGrab.HasSelectedInteractable;
    }

    private void Shoot()
    {
        if (!canShoot) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        
        float currentForce = isTwoHanded ? shootForce * 1.5f : shootForce;
        bulletRb.AddForce(muzzlePoint.forward * currentForce, ForceMode.Impulse);
        
        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}