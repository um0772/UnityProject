using UnityEngine;

public class Player : MonoBehaviour
{


    public GameObject destructionFX; // 파괴 시 생성할 파티클 효과
    public int life = 3; // 플레이어의 초기 생명력
    public int score; // 플레이어의 점수
    public GameObject boomEffect;
    public int boom;
    public int maxBoom;
    public bool isBoomTime;

    public static Player instance; // 싱글톤 인스턴스
    private GameManager gameManager;


    void Update()
    {
        Boom();
    }
    private void Awake()
    {
        if (instance == null)
            instance = this; // 인스턴스가 없으면 자신을 할당

        gameManager = FindObjectOfType<GameManager>(); // GameManager를 찾아서 할당
    }


    public void AddScore(int points)
    {
        score += points; // 점수 증가
        gameManager?.UpdateScore(score); // GameManager를 통해 UI 업데이트 (null 체크 추가)
    }
    // 점수를 초기화하는 메서드
    public void ResetScore()
    {
        score = 0;
        gameManager.UpdateScore(score);
    }

    // 플레이어가 데미지를 받는 메서드
    public void GetDamage(int damage)
    {
        life -= damage; // 생명력 감소
        gameManager.UpdateLifeIcon(life); // GameManager를 통해 UI 업데이트

        if (life > 0)
        {
            gameManager.RespawnPlayer(); // 생명력이 0이 아니면 플레이어 부활
        }
        else
        {
            life = 0; // life가 음수가 되지 않도록 보정
            Destruction(); // 생명력이 0 이하면 플레이어 파괴
        }

    }

    public void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.GetDamage(1000);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");
        for (int index = 0; index < bullets.Length; index++)
        {
            Destroy(bullets[index]);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Power":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                        

                    break;
            }
            Destroy(collision.gameObject);
        }
    }
    
    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    // 플레이어를 파괴하는 메서드
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); // 파괴 효과 생성
        gameObject.SetActive(false); // 플레이어 비활성화
        gameManager.GameOver(); // 게임 오버 처리는 GameManager에서 호출
    }
}
