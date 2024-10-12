using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This script generates an enemy wave. It defines how many enemies will be emerging, their speed and emerging interval. 
/// It also defines their shooting mode. It defines their moving path.
/// </summary>
[System.Serializable]
public class Shooting
{
    [Range(0, 100)]
    [Tooltip("probability with which the ship of this wave will make a shot")]
    public int shotChance;

    [Tooltip("min and max time from the beginning of the path when the enemy can make a shot")]
    public float shotTimeMin, shotTimeMax;
}

public class Wave : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Enemy's prefab")]
    public GameObject enemy;

    [Tooltip("a number of enemies in the wave")]
    public int count;

    [Tooltip("path passage speed")]
    public float speed;

    [Tooltip("time between emerging of the enemies in the wave")]
    public float timeBetween;

    [Tooltip("points of the path. delete or add elements to the list if you want to change the number of the points")]
    public Transform[] pathPoints;

    [Tooltip("whether 'Enemy' rotates in path passage direction")]
    public bool rotationByPath;

    [Tooltip("if loop is activated, after completing the path 'Enemy' will return to the starting point")]
    public bool Loop;

    [Tooltip("color of the path in the Editor")]
    public Color pathColor = Color.yellow;
    public Shooting shooting;

    [Tooltip("if testMode is marked the wave will be re-generated after 3 sec")]
    public bool testMode;
    #endregion

    private void Start()
    {
        StartCoroutine(CreateEnemyWave());
    }

    IEnumerator CreateEnemyWave()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offScreenStartPosition = GetOffScreenStartPosition();
            Vector3 offScreenEndPosition = GetOffScreenEndPosition();
            GameObject newEnemy = Instantiate(enemy, offScreenStartPosition, Quaternion.identity);
            FollowThePath followComponent = newEnemy.GetComponent<FollowThePath>();
            followComponent.path = CreatePathWithStartAndEndPoints(offScreenStartPosition, offScreenEndPosition);
            followComponent.speed = speed;
            followComponent.rotationByPath = rotationByPath;
            followComponent.loop = Loop;
            followComponent.SetPath();
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.shotChance = shooting.shotChance;
            enemyComponent.shotTimeMin = shooting.shotTimeMin;
            enemyComponent.shotTimeMax = shooting.shotTimeMax;
            newEnemy.SetActive(true);
            yield return new WaitForSeconds(timeBetween);
        }
        if (testMode)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(CreateEnemyWave());
        }
        else if (!Loop)
        {
            Destroy(gameObject);
        }
    }

    Vector3 GetOffScreenStartPosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenPosition = new Vector3();
        float randomValue = UnityEngine.Random.value;

        // Determine which side of the screen to spawn from (left or right)
        if (randomValue < 0.5f)
        {
            // Left side
            screenPosition = new Vector3(-0.1f, UnityEngine.Random.Range(0.0f, 1.0f), 10);
        }
        else
        {
            // Right side
            screenPosition = new Vector3(1.1f, UnityEngine.Random.Range(0.0f, 1.0f), 10);
        }

        return mainCamera.ViewportToWorldPoint(screenPosition);
    }

    Vector3 GetOffScreenEndPosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenPosition = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), 1.1f, 10);
        return mainCamera.ViewportToWorldPoint(screenPosition);
    }

    Transform[] CreatePathWithStartAndEndPoints(Vector3 startPoint, Vector3 endPoint)
    {
        List<Transform> newPathPoints = new List<Transform>();

        // Create a temporary transform for the start point
        GameObject tempStartPoint = new GameObject("TempStartPoint");
        tempStartPoint.transform.position = startPoint;
        newPathPoints.Add(tempStartPoint.transform);

        // Add the rest of the path points
        foreach (Transform point in pathPoints)
        {
            newPathPoints.Add(point);
        }

        // Add the end point
        GameObject tempEndPoint = new GameObject("TempEndPoint");
        tempEndPoint.transform.position = endPoint;
        newPathPoints.Add(tempEndPoint.transform);

        return newPathPoints.ToArray();
    }

    void OnDrawGizmos()
    {
        if (pathPoints.Length == 0) return;

        Vector3 offScreenStartPosition = GetOffScreenStartPosition();
        Vector3 offScreenEndPosition = GetOffScreenEndPosition();
        List<Vector3> pathList = new List<Vector3> { offScreenStartPosition };
        foreach (Transform point in pathPoints)
        {
            pathList.Add(point.position);
        }
        pathList.Add(offScreenEndPosition);

        Vector3[] pathPositions = pathList.ToArray();
        Vector3[] newPathPositions = CreatePoints(pathPositions);
        Vector3 previousPosition = Interpolate(newPathPositions, 0);
        Gizmos.color = pathColor;
        int SmoothAmount = pathPositions.Length * 20;
        for (int i = 1; i <= SmoothAmount; i++)
        {
            float t = (float)i / SmoothAmount;
            Vector3 currentPosition = Interpolate(newPathPositions, t);
            Gizmos.DrawLine(previousPosition, currentPosition);
            previousPosition = currentPosition;
        }
    }

    Vector3 Interpolate(Vector3[] path, float t)
    {
        int numSections = path.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;
        Vector3 a = path[currPt];
        Vector3 b = path[currPt + 1];
        Vector3 c = path[currPt + 2];
        Vector3 d = path[currPt + 3];
        return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
    }

    Vector3[] CreatePoints(Vector3[] path)
    {
        Vector3[] pathPositions;
        Vector3[] newPathPos;
        int dist = 2;
        pathPositions = path;
        newPathPos = new Vector3[pathPositions.Length + dist];
        Array.Copy(pathPositions, 0, newPathPos, 1, pathPositions.Length);
        newPathPos[0] = newPathPos[1] + (newPathPos[1] - newPathPos[2]);
        newPathPos[newPathPos.Length - 1] = newPathPos[newPathPos.Length - 2] + (newPathPos[newPathPos.Length - 2] - newPathPos[newPathPos.Length - 3]);
        if (newPathPos[1] == newPathPos[newPathPos.Length - 2])
        {
            Vector3[] LoopSpline = new Vector3[newPathPos.Length];
            Array.Copy(newPathPos, LoopSpline, newPathPos.Length);
            LoopSpline[0] = LoopSpline[LoopSpline.Length - 3];
            LoopSpline[LoopSpline.Length - 1] = LoopSpline[2];
            newPathPos = new Vector3[LoopSpline.Length];
            Array.Copy(LoopSpline, newPathPos, LoopSpline.Length);
        }
        return (newPathPos);
    }
}
