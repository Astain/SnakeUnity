using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid lg)
    {
        this.levelGrid = lg;
    }
    private void Awake()
    {
        //Start on the middle of the grid
        gridPosition = new Vector2Int(10, 10);

        //Default movement direction
        gridMoveDirection = new Vector2Int(1, 0);

        //Snake can move 2 per second
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodySize = 0;

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
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            //Move the snake
            gridPosition += gridMoveDirection;
       
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if(snakeAteFood)
            {
                snakeBodySize += 1;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);

            //Face the snake
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

            UpdateSnakeBodyParts();

        }
    }
    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeMovePositionList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    public List<Vector2Int> GetFullSnakeGridPosition()
    {
        List<Vector2Int> gridPosList = new List<Vector2Int>() { gridPosition };
        gridPosList.AddRange(snakeMovePositionList);
        return gridPosList;
    }

    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodyprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}
