using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet6 : Bullet
{
    private LineRenderer pathManager;
    private Vector3[] path;
    private Dictionary<int, int> deltaSpeed; // Key: path의 node index, Value: speed의 변화량
    private Tower tower;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pathManager = FindObjectOfType<LineRenderer>();
        path = new Vector3[pathManager.positionCount];
        pathManager.GetPositions(path);
        tower = transform.parent.GetComponent<Tower>();

        UpdateDeltaSpeed();
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            UpdateDeltaSpeed();
            transform.hasChanged = false;
        }
    }

    private void UpdateDeltaSpeed()
    {
        direction = transform.rotation * Vector3.forward;
        deltaSpeed = new Dictionary<int, int>();
        for (int i = 0; i < path.Length - 1; i++)
        {
            Vector3 pathDirection = (path[i + 1] - path[i]).normalized;
            if (Vector3.Angle(direction, path[i] - transform.position) == 0)
            {
                if (direction == pathDirection)
                {
                    deltaSpeed.Add(i, 1);
                }
                else if (direction == -pathDirection)
                {
                    deltaSpeed.Add(i, -1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Wind Tower 피격 처리는 하지 않음
    }

    public float GetDeltaSpeed(int node)
    {
        int value = 0;
        deltaSpeed.TryGetValue(node, out value);
        return value * tower.GetDamage() / 10f;
    }
}
