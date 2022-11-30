using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public bool Opened = false;

    private int randomWeapon;
    private GameObject player;
    private List<string> usedWeapons;
    new private BoxCollider2D collider;
    private Vector2 mousePos;

    private void Awake()
    {
        usedWeapons = new List<string>();
        player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.E) && collider.OverlapPoint(mousePos))
        {
            animator.SetBool("IsOpening", true);
            RollWeapon();
            Opened = true;
            Destroy(this);
        }
    }

    private void RollWeapon()
    {
        randomWeapon = Random.Range(1, player.transform.childCount);
        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (i == randomWeapon && player.transform.GetChild(i) != player.transform.Find(player.GetComponent<PlayerMovement>().activeWeapon.name)
                && !usedWeapons.Contains(player.transform.GetChild(i).gameObject.name))
            {
                player.transform.GetChild(i).gameObject.SetActive(true);
                usedWeapons.Add(player.transform.GetChild(i).gameObject.name);
            }
            else if (i == randomWeapon && player.transform.GetChild(i) == player.transform.Find(player.GetComponent<PlayerMovement>().activeWeapon.name))
                RollWeapon();
            else
                player.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}