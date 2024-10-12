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

    private bool isPaused = false; // 게임 일시정지 상태 추가
    private bool gameIsOver = false; // 게임 오버 상태
  

    private void Start()
    {
      
        ResetGame();
        gameOverSet.SetActive(false); // 게임 시작 시 gameOverSet 비활성화
        pauseMenuSet.SetActive(false); // 게임 시작 시 pauseMenuSet 비활성화
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
                scoreText.text = string.Format("{0:n0}", playerLogic.score); // 점수 UI 업데이트
            }
        }
    }

    // 게임 초기화를 위한 메서드
    public void ResetGame()
    {
        if (player != null)
        {
            Player playerLogic = player.GetComponent<Player>();
            if (playerLogic != null)
            {
                playerLogic.ResetScore(); // 점수 초기화
            }
        }
        UpdateScore(0); // UI를 0으로 초기화
    }

    public void UpdateScore(int score)
    {
        scoreText.text = string.Format("{0:n0}", score); // 점수 UI 업데이트
    }

    // 이 부분을 public으로 변경하여 외부 클래스에서 접근할 수 있도록 함
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0); // 모든 이미지를 투명하게 만듦
        }
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1); // 생명력에 맞게 이미지를 반투명하게 만듦
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0); // 모든 이미지를 투명하게 만듦
        }
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1); // 생명력에 맞게 이미지를 반투명하게 만듦
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
        player.transform.position = Vector3.down * 3.5f; // 적절한 위치로 리스폰 위치 설정
        player.SetActive(true);
    }

    public void GameOver()
    {
        gameIsOver = true;
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1); // GameScene의 인덱스가 1이라고 가정
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0); // StartMenu Scene의 인덱스가 0이라고 가정
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // 게임 시간을 멈춤
        pauseMenuSet.SetActive(true); // 일시정지 메뉴 활성화
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // 게임 시간을 재개
        pauseMenuSet.SetActive(false); // 일시정지 메뉴 비활성화
    }

    public void ExitToMainMenuFromPause()
    {
        Time.timeScale = 1; // 게임 시간을 재개
        SceneManager.LoadScene(0); // StartMenu Scene의 인덱스가 0이라고 가정
    }
}
