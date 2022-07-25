// using System;
using UnityEngine;

// 적 오브젝트입니다.
public class Enemy : MonoBehaviour
{
    public int hp;
    public float speed;

    private PlayerInfo playerInfo;
    private LineRenderer pathManager;
    private EnemyManager enemyManager;
    private Rigidbody rb;
    private Vector3[] path;
    private int currentNode = 0;
    private int nextNode = 1;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        enemyManager = FindObjectOfType<EnemyManager>();
        rb = GetComponent<Rigidbody>();
        enemyManager.AddEnemy(this);
        pathManager = FindObjectOfType<LineRenderer>();
        path = new Vector3[pathManager.positionCount];
        pathManager.GetPositions(path);
        transform.position = path[currentNode];
        transform.GetChild(0).rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        // 경로 따라가기
        Vector3 position = rb.position;
        float distance = (path[nextNode] - position).magnitude;
        float move = Time.deltaTime * (speed > 0 ? speed : 0);
        if (move >= distance)
        {
            currentNode++; nextNode++;
            if (nextNode == path.Length)
            {
                playerInfo.SubtractLife(hp);
                Destroy(gameObject);
                return;
            }
            position = path[currentNode];
            move = distance;
        }
        Vector3 direction = (path[nextNode] - path[currentNode]).normalized;
        position += direction * move;
        rb.MovePosition(position);
    }

    void OnDestroy()
    {
        enemyManager.RemoveEnemy(this);
    }

    public void SubtractHp(int delta)
    {
        hp -= delta;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetLocation()
    {
        return 100 * currentNode + (path[currentNode] - transform.position).magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet6 bullet;
            if (other.TryGetComponent<Bullet6>(out bullet))
            {
                speed -= bullet.GetDamage() / 10f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet6 bullet;
            if (other.TryGetComponent<Bullet6>(out bullet))
            {
                speed += bullet.GetDamage() / 10f;
            }
        }
    }
}
