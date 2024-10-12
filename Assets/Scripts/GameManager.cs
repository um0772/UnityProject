using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public GameObject pauseMenuSet;

    private bool isPaused = false; // ���� �Ͻ����� ���� �߰�
    private bool gameIsOver = false; // ���� ���� ����
  

    private void Start()
    {
      
        ResetGame();
        gameOverSet.SetActive(false); // ���� ���� �� gameOverSet ��Ȱ��ȭ
        pauseMenuSet.SetActive(false); // ���� ���� �� pauseMenuSet ��Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (player != null)
        {
            Player playerLogic = player.GetComponent<Player>();
            if (playerLogic != null)
            {
                scoreText.text = string.Format("{0:n0}", playerLogic.score); // ���� UI ������Ʈ
            }
        }
    }

    // ���� �ʱ�ȭ�� ���� �޼���
    public void ResetGame()
    {
        if (player != null)
        {
            Player playerLogic = player.GetComponent<Player>();
            if (playerLogic != null)
            {
                playerLogic.ResetScore(); // ���� �ʱ�ȭ
            }
        }
        UpdateScore(0); // UI�� 0���� �ʱ�ȭ
    }

    public void UpdateScore(int score)
    {
        scoreText.text = string.Format("{0:n0}", score); // ���� UI ������Ʈ
    }

    // �� �κ��� public���� �����Ͽ� �ܺ� Ŭ�������� ������ �� �ֵ��� ��
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0); // ��� �̹����� �����ϰ� ����
        }
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1); // ����¿� �°� �̹����� �������ϰ� ����
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0); // ��� �̹����� �����ϰ� ����
        }
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1); // ����¿� �°� �̹����� �������ϰ� ����
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnPlayerDelayed());
    }

    IEnumerator RespawnPlayerDelayed()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2f);
        player.transform.position = Vector3.down * 3.5f; // ������ ��ġ�� ������ ��ġ ����
        player.SetActive(true);
    }

    public void GameOver()
    {
        gameIsOver = true;
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1); // GameScene�� �ε����� 1�̶�� ����
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0); // StartMenu Scene�� �ε����� 0�̶�� ����
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // ���� �ð��� ����
        pauseMenuSet.SetActive(true); // �Ͻ����� �޴� Ȱ��ȭ
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // ���� �ð��� �簳
        pauseMenuSet.SetActive(false); // �Ͻ����� �޴� ��Ȱ��ȭ
    }

    public void ExitToMainMenuFromPause()
    {
        Time.timeScale = 1; // ���� �ð��� �簳
        SceneManager.LoadScene(0); // StartMenu Scene�� �ε����� 0�̶�� ����
    }
}
