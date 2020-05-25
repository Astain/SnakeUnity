using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax; 

    private void Awake()
    {
        //Start on the middle of the grid
        gridPosition = new Vector2Int(10, 10);

        //Default movement direction
        gridMoveDirection = new Vector2Int(1, 0);

        //Snake can move 2 per second
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;

    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();       
    }
    private void HandleInput()
    {
        if (gridMoveDirection.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gridMoveDirection.y = -1;
                gridMoveDirection.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gridMoveDirection.y = 1;
                gridMoveDirection.x = 0;
            }
        }
        else if (gridMoveDirection.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gridMoveDirection.x = 1;
                gridMoveDirection.y = 0;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;

            //Move the snake
            transform.position = new Vector3(gridPosition.x, gridPosition.y);

            //Face the snake
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
        }
    }
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
