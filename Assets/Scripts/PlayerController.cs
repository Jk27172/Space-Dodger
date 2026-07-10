using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 5f;

    public ParticleSystem boosterFlame;

    Rigidbody2D rb;

    private float elapsedTime = 0f;
    private int score = 0;
    public float scoreMultiplier = 10f;
    
    public UIDocument uiDocument;

    private Label scoreText;
    private Label gameOverText;
    private Label highScoreText;
   

    public GameObject explosionEffect;

    private Button restartButton;

    public AudioSource engineSound;

    public InputAction moveForward;
    public InputAction lookPosition;

    public bool playerIsAlive = true;




    void Start()
    {
        UnityEngine.Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);

        gameOverText = uiDocument.rootVisualElement.Q<Label>("GameoverLabel");
        gameOverText.style.display = DisplayStyle.None;
      

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

        moveForward.Enable();
        lookPosition.Enable();

    }

    
    void Update()
    {
        UpdateScore();
        MovePlayer();

        Vector3 pos = transform.position;
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);

        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    void UpdateScore()
    {
        if (!playerIsAlive)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void MovePlayer()
    {
        if (moveForward.IsPressed())
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
           
        }

        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.Play(true);
            engineSound.Play();
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.Stop(false);
            engineSound.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (!playerIsAlive)
        {
            return;
        }

        playerIsAlive = false;

        

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        boosterFlame.Stop();
        engineSound.Stop();

        GetComponent<SpriteRenderer>().enabled = false;

        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

        explosion.GetComponent<AudioSource>()?.Play();

        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        highScoreText.text = "High Score: " + highScore;

        UnityEngine.Cursor.visible = true;

        gameOverText.style.display = DisplayStyle.Flex;
        restartButton.style.display = DisplayStyle.Flex;

        StartCoroutine(FreezeGameAfterDelay(1.5f));
    }

    IEnumerator FreezeGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    void ReloadScene()
    {
        UnityEngine.Cursor.visible = true;

        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
