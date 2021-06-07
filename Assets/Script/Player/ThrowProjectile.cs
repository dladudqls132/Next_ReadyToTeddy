using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{
    private LineRenderer lineVisual;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform throwPos;
    [SerializeField] private GameObject cursor;
    [SerializeField] private LayerMask layer;
    [SerializeField] private int lineSegment;
    [SerializeField] private float throwDistance;

    private Camera cam;
    private Vector3 velocity;
    public void SetProjectile(Rigidbody temp) { projectile = temp; } 

    private void Start()
    {
        cam = Camera.main;
        lineVisual = this.GetComponent<LineRenderer>();
        lineVisual.positionCount = lineSegment;
    }

    public void ResetInfo()
    {
        lineVisual.enabled = false;
        cursor.SetActive(false);
    }

    public void LaunchProjectile()
    {
        Rigidbody temp = Instantiate(projectile, throwPos.position, Quaternion.identity);
        temp.velocity = velocity;
        temp.GetComponent<Projectile>().SetIsThrown(true);

        ResetInfo();
    }

    public bool AimingProjectile()
    {
        //if(projectile != GameManager.Instance.GetPlayer().GetWeaponGameObject())
        //{
        //    projectile = GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Projectile>().projectile.GetComponent<Rigidbody>();
        //}

        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            cursor.SetActive(true);
            lineVisual.enabled = true;
            if (hit.distance < throwDistance)
            {
                lineVisual.enabled = true;
                cursor.GetComponent<SpriteRenderer>().color = Color.green;
                cursor.SetActive(true);
                cursor.transform.position = hit.point + hit.normal * 0.02f;
                cursor.transform.rotation = Quaternion.LookRotation(hit.normal);
              
                Vector3 Vo = CalculateVelocity(hit.point, throwPos.position, Mathf.Clamp(Vector3.Distance(hit.point, throwPos.position) / 8.0f, 0.25f, 1.0f));

                Visualize(Vo, Mathf.Clamp(Vector3.Distance(hit.point, throwPos.position) / 8.0f, 0.25f, 1.0f));
                velocity = Vo;
                //transform.rotation = Quaternion.LookRotation(Vo);

                //if (Input.GetMouseButtonUp(0))
                //{
                //    Rigidbody temp = Instantiate(bulletPrefab, throwPos.position, Quaternion.identity);
                //    temp.velocity = Vo;
                //    //temp.useGravity = true;
                //    //LaunchBulletFromPool(throwPos.position, Quaternion.identity, Vo);
                //    //getBulletNum--;
                //    //GameManager.Instance.soundController.PlaySFXOneShot("Sound_ingame_throw_2", 0.25f, Random.Range(0.85f, 0.95f));
                //}

                return true;
            }
            else
            {
                cursor.GetComponent<SpriteRenderer>().color = Color.red;
                cursor.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 152 / 255f);
                cursor.transform.position = hit.point + Vector3.up * 0.02f;
                cursor.transform.rotation = Quaternion.LookRotation(hit.normal);
                lineVisual.enabled = false;

                return false;
            }
        }
        else
        {
            cursor.SetActive(false);
            lineVisual.enabled = false;

            return false;
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    void Visualize(Vector3 vo, float speed)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment * speed);

            RaycastHit hit;

            if (Physics.Raycast(pos, (CalculatePosInTime(vo, i / (float)lineSegment * speed) - CalculatePosInTime(vo, (i - 1) / (float)lineSegment * speed)).normalized, out hit, 0.6f, layer))
            {
                for (int j = i; j < lineSegment; j++)
                {
                    if (i - 1 >= 0)
                        lineVisual.SetPosition(j, lineVisual.GetPosition(i - 1));
                }

                lineVisual.SetPosition(i, hit.point);
                cursor.transform.position = hit.point + hit.normal * 0.05f;
                cursor.transform.rotation = Quaternion.LookRotation(hit.normal);
                break;
            }

            lineVisual.SetPosition(i, pos);
        }
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = throwPos.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + throwPos.position.y;

        result.y = sY;

        return result;
    }

}
