using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRow : MonoBehaviour
{
    public bool fromRight;
    public float rowInterval;

    public List<GameObject> children;
    public int index = 0;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in transform)
        {
            children.Add(item.gameObject);
        }
        if (fromRight)
        {
            children.Reverse();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            if (index == children.Count)
            {
                index = 0;
            }
            StartCoroutine(ActivateInOrder(children[index]));
        }
    }

    IEnumerator ActivateInOrder(GameObject fire)
    {
        fire.GetComponent<Activate_Trap>().ActivateFireTrap();
        isActive = true;
        yield return new WaitForSeconds(0);
    }

}
