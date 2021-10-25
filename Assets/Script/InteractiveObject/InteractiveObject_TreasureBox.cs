using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject_TreasureBox : MonoBehaviour
{
    [SerializeField] private GameObject prefab_dropWeapon;
    private GameObject dropWeapon;
    private bool isOpened;
    private Image f;

    // Start is called before the first frame update
    void Start()
    {
        dropWeapon = Instantiate(prefab_dropWeapon, this.transform);
        dropWeapon.SetActive(false);
        f = GameObject.Find("F").GetComponent<Image>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpened) return;
        
        Transform temp = other.transform;

        if(temp.CompareTag("Player"))
        {
            if(Vector3.Dot(Camera.main.transform.forward, (this.transform.position - Camera.main.transform.position).normalized) >= 0.9f)
            {
                f.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.GetPlayer().GetInventory().AddWeapon(dropWeapon);
                    isOpened = true;
                    this.gameObject.SetActive(false);
                    f.enabled = false;
                }
            }
            else
                f.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        f.enabled = false;
    }
}
