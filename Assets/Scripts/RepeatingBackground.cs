using UnityEngine;

/// <summary>
/// This script attaches to ‘Background’ object, and would move it up if the object went down below the viewport border. 
/// This script is used for creating the effect of infinite movement. 
/// </summary>

public class RepeatingBackground : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float scrollRange = 40.96f;
    [SerializeField]
    private float moveSpeed = 3.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.down;
    private void Start()
    {
        // 이미지 크기에 맞게 초기 위치 설정
        float imageHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        scrollRange = imageHeight;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (transform.position.y < -scrollRange) //if sprite goes down below the viewport move the object up above the viewport
        {
            Vector3 newPos = transform.position;
            newPos.y += 2 * scrollRange; // 정확히 이미지 두 배 높이만큼 이동
            transform.position = newPos;
        }
    }

    
}
