using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointGenerator : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private GameObject[] ground;
    private GameObject[] walls;
    private GameObject[] JumpPoints;
    private GameObject wall;
    private int wallMinDistance = int.MaxValue;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        ground = GameObject.FindGameObjectsWithTag("Ground");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        JumpPoints = new GameObject[ground.Length * 2];
        for (int i = 0; i < ground.Length; i++)
        {
            if (i == 0)
            {
                JumpPoints[0] = new GameObject();
                JumpPoints[0].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[0].transform.parent = ground[i].transform;

                wall = new GameObject();
                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[0].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[0].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[0].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[0].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x + boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }

                JumpPoints[1] = new GameObject();
                JumpPoints[1].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[1].transform.parent = ground[i].transform;

                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[i].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[1].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[1].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[1].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x - boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }
            }
            else if (i == 1)
            {
                JumpPoints[2] = new GameObject();
                JumpPoints[2].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[2].transform.parent = ground[i].transform;

                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[2].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[2].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[2].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[2].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x + boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }

                JumpPoints[3] = new GameObject();
                JumpPoints[3].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[3].transform.parent = ground[i].transform;

                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[3].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[3].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[3].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[3].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x - boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }
            }
            else
            {
                JumpPoints[i * 2 - 1] = new GameObject();
                JumpPoints[i * 2 - 1].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[i * 2 - 1].transform.parent = ground[i].transform;

                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[i * 2 - 1].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[i * 2 - 1].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[i * 2 - 1].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[i * 2 - 1].transform.position = new Vector2(ground[i].transform.position.x - ground[i].GetComponent<BoxCollider2D>().bounds.extents.x + boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }

                JumpPoints[i * 2] = new GameObject();
                JumpPoints[i * 2].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                JumpPoints[i * 2].transform.parent = ground[i].transform;

                wallMinDistance = int.MaxValue;
                for (int j = 0; j < walls.Length; j++)
                {
                    if (Vector2.Distance(walls[j].transform.position, JumpPoints[i * 2].transform.position) < wallMinDistance)
                    {
                        wall = walls[j];
                        wallMinDistance = (int)Vector2.Distance(walls[j].transform.position, JumpPoints[i * 2].transform.position);
                    }
                }

                if (Vector2.Distance(JumpPoints[i * 2].transform.position, wall.transform.position) <= wall.GetComponent<BoxCollider2D>().bounds.extents.x + wall.GetComponent<BoxCollider2D>().bounds.extents.y)
                {
                    JumpPoints[i * 2].transform.position = new Vector2(ground[i].transform.position.x + ground[i].GetComponent<BoxCollider2D>().bounds.extents.x - boxCollider.bounds.extents.x * 2,
                    ground[i].transform.position.y + ground[i].GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f);
                }
            }
        }
    }
}
