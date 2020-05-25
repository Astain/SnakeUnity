using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{

    private Vector2Int foodGridPosition;
    private int width;
    private int height;
    private GameObject foodGameObject;
    private Snake snake;

    public void Setup(Snake s)
    {
        this.snake = s;

        //Spawn it here instead of the constructor due a dependece with Snake Object.
        SpawnFood();
    }
    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (foodGridPosition == snake.GetGridPosition());
        foodGameObject = new GameObject();
        SpriteRenderer foodSpriteRenderer = foodGameObject.AddComponent<SpriteRenderer>();

        foodSpriteRenderer.sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public void SnakeMoved(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
        }
    }
}
