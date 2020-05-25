using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
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
        gridMoveDirection = Direction.Right;

        //Snake can move 2 per second
        gridMoveTimerMax = .25f;
        gridMoveTimer = gridMoveTimerMax;

        snakeMovePositionList = new List<SnakeMovePosition>();
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
        if (gridMoveDirection != Direction.Up && gridMoveDirection != Direction.Down)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gridMoveDirection = Direction.Down;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gridMoveDirection = Direction.Up;
            }
        }
        else if (gridMoveDirection != Direction.Left && gridMoveDirection != Direction.Right)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gridMoveDirection = Direction.Left;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SnakeMovePosition snakeMovePreviousPosition = null;
            if(snakeMovePositionList.Count>0)
            {
                snakeMovePreviousPosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(snakeMovePreviousPosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            //Move the snake
            Vector2Int gridMoveDirectionVector;
            switch(gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            gridPosition += gridMoveDirectionVector;
       
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
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

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
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPosList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPosList;
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodyprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: // Currently going Up
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 0 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 0 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                    }
                    break;
                case Direction.Down: // Currently going Down
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 180 - 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 180 + 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Left: // Currently going to the Left
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = +90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Right: // Currently going to the Right
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = -45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousPosition;
        private Vector2Int position;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousPosition, Vector2Int position, Direction direction)
        {
            this.previousPosition = previousPosition;
            this.position = position;
            this.direction = direction;
        }
        public Vector2Int GetGridPosition ()
        {
            return position;
        }
        public Direction GetDirection()
        {
            return this.direction;
        }
        public Direction GetPreviousDirection()
        {
            if(previousPosition == null)
            {
                return Direction.Right;
            }
            return previousPosition.GetDirection();
        }
    }
}
