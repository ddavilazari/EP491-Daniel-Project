using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 120f; // Time limit in seconds
    private float timer = 0f;
    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            timer += Time.deltaTime;
            if (timer >= timeLimit)
            {
                Debug.Log("Time's up!");
                // Stop the game when the time limit is reached
                Time.timeScale = 0f;
                isGameOver = true;
            }
        }
    }
}
