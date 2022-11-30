using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Trap : MonoBehaviour
{
    public Transform Trap_Pos;
    public GameObject prefab;
    public string soundToPlay;

    public bool isFireTrap;
    public bool isAutomatic;
    public float automaticRate;
    public float automaticDelay;

    private bool isActive;
    private Transform fire;
    private new Light light;

    public bool isPartOfFireRow;
    private FireRow fireRowParent;
    private void Start()
    {
        if (isPartOfFireRow)
        {
            fireRowParent = transform.parent.gameObject.GetComponent<FireRow>();
        }

        if (isFireTrap)
        {
            fire = transform.GetChild(0);

            light = transform.GetChild(1).gameObject.GetComponent<Light>();
            light.enabled = false;

            fire.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            fire.gameObject.GetComponent<Animator>().SetBool("IsActive", false);

            if (isAutomatic)
            {
                Invoke("StartMyCoroutine", automaticDelay);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFireTrap && !isAutomatic)
        {
            if (collision.gameObject.tag == "Player" && !isActive)
            {
                FindObjectOfType<AudioManager>().Play("PressurePlate");
                ActivateFireTrap();
            }
        }
        else if(!isFireTrap)
        {
            if (collision.gameObject.tag == "Player")
            {
                FindObjectOfType<AudioManager>().Play("PressurePlate");
                FindObjectOfType<AudioManager>().Play(soundToPlay);
                Instantiate(prefab, new Vector3(Trap_Pos.position.x, Trap_Pos.position.y, 0f), Quaternion.identity);
            }
        }
    }
    public void ActivateFireTrap()
    {
        fire.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        fire.gameObject.GetComponent<Animator>().SetBool("IsActive", true);
        isActive = true;
        FindObjectOfType<AudioManager>().Play(soundToPlay);
        light.enabled = true;
        StartCoroutine(SelfDestruct());
        if (isPartOfFireRow)
        {
            StartCoroutine(MakeInactive());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1.5f);
       
        isActive = false;
        fire.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        fire.gameObject.GetComponent<Animator>().SetBool("IsActive", false);
        light.enabled = false;
    }
    IEnumerator MakeInactive()
    {
        yield return new WaitForSeconds(fireRowParent.rowInterval);
        fireRowParent.isActive = false;
        fireRowParent.index++;
    }
    IEnumerator AutomaticFireTrap()
    {  
        while (isAutomatic)
        {
            yield return new WaitForSeconds(automaticRate);
            ActivateFireTrap();
        }
    }

    void StartMyCoroutine()
    {
        ActivateFireTrap();
        StartCoroutine(AutomaticFireTrap());
    }
}