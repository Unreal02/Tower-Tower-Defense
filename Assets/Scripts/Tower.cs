using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 타워 오브젝트입니다.
// mouseCursor에서 타워 설치하기 전에는 비활성화 상태입니다.
// 타워를 설치하는 순간 활성화됩니다.
public class Tower : MonoBehaviour
{
    [Header("타워 정보")]
    public int idx; // 타워 인덱스 (종류를 나타냄)
    public int[] cost; // 가격
    public float[] radius; // 공격 반경
    public float[] delay; // 공격 딜레이 시간

    [Header("투사체 정보")]
    public GameObject[] bullet; // 투사체
    public int[] damage; // 공격력
    public float[] speed; // 투사체 발사 속도
    public bool[] targeting; // 투사체가 목표를 따라가는지
    public float[] life; // 투사체 지속 시간

    // 보너스는 (원래 값 + 보너스 값)으로 적용됨
    private float radiusBonus;
    private float delayBonus;
    private int damageBonus;
    private float speedBonus;

    private Dictionary<int, int> stackedTower = new Dictionary<int, int>(); // 쌓인 타워의 목록 (맨 아래 타워에만 존재함). key: idx, value: 개수
    private HashSet<int> activatedSynergies = new HashSet<int>(); // 현재 활성화된 시너지

    private Component radiusSphere; // 반경을 나타내는 투명한 구
    private MouseCursor mouseCursor; // 마우스 커서 오브젝트
    private EnemyManager enemyManager;
    private SynergyManager synergyManager;
    private bool select; // 오브젝트가 선택되었는지 나타냄
    private bool attackable; // 공격 딜레이가 지나서 공격 가능한가
    private int level; // 현재 타워 레벨

    private Tower upperTower;
    private Tower lowerTower;

    // Start is called before the first frame update
    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
        enemyManager = FindObjectOfType<EnemyManager>();
        synergyManager = FindObjectOfType<SynergyManager>();
        radiusSphere = transform.GetChild(1);
        attackable = true;
        level = 0;

        UpdateSynergy(); // Start가 OnInstallTower보다 늦게 불려서 OnInstallTower에서 UpdateSynergy를 호출하면 안 되더라...
    }

    // Update is called once per frame
    void Update()
    {
        if (attackable)
        {
            // todo: 타겟 설정 바꾸는 기능
            Enemy target = GetTarget(e => e.GetLocation()); // 가장 앞에 있는 적
            // Enemy target = GetTarget(e => -(e.transform.position - transform.position).magnitude); // 가장 가까운 적
            if (target != null)
            {
                Bullet newBullet = Instantiate(bullet[level], transform.position, transform.rotation).GetComponent<Bullet>();
                newBullet.SetTarget(target);
                newBullet.SetBulletInfo(damage[level] + damageBonus, speed[level] + speedBonus, targeting[level], life[level]);
                Invoke("SetAttackable", delay[level] + delayBonus);
                attackable = false;
            }
        }
    }

    void SetAttackable()
    {
        attackable = true;
    }

    public void SetSelect(bool b)
    {
        select = b;
        radiusSphere.gameObject.SetActive(b);
    }

    public int GetCost() { return cost[level]; }
    public int GetNextCost() { return cost[level + 1]; }
    public float GetRadius() { return radius[level] + radiusBonus; }
    public float GetDelay() { return delay[level] + delayBonus; }
    public float GetDamage() { return damage[level] + damageBonus; }
    public int GetLevel() { return level; }
    public Dictionary<int, int> GetStackedTower()
    {
        if (lowerTower == null) return stackedTower;
        else return lowerTower.GetStackedTower();
    }
    public void SetUpperTower(Tower t) { upperTower = t; }

    public void AddLevel() { level++; UpdateRadiusSphere(); }

    private void UpdateRadiusSphere()
    {
        radiusSphere.transform.localScale = 2 * (radius[level] + radiusBonus) * new Vector3(1, 1, 1);
    }

    private Enemy GetTarget(Func<Enemy, float> func) // func 함수값이 최대인 적을 선택
    {
        HashSet<Enemy> set = enemyManager.GetEnemySet();
        Enemy target = null;
        float targetKey = Mathf.NegativeInfinity;
        if (set.Count > 0)
        {
            foreach (Enemy e in set)
            {
                float distance = (e.transform.position - transform.position).magnitude;
                float key = func(e);
                if (distance <= radius[level] + radiusBonus && key > targetKey)
                {
                    target = e;
                    targetKey = key;
                }
            }
        }

        return target;
    }

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            mouseCursor.OnClickTower(gameObject);
    }

    public void OnInstallTower()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Tower");
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1, layerMask))
        {
            lowerTower = hit.transform.GetComponent<Tower>();
            lowerTower.SetUpperTower(this);
        }
        AddTowerToStackedTower(idx);
    }

    public void AddTowerToStackedTower(int idx)
    {
        if (lowerTower != null) lowerTower.AddTowerToStackedTower(idx);
        else
        {
            if (stackedTower.ContainsKey(idx)) stackedTower[idx] += 1;
            else stackedTower.Add(idx, 1);
        }
    }

    public void UpdateSynergy()
    {
        if (lowerTower != null) lowerTower.UpdateSynergy();
        var dict = GetStackedTower();
        activatedSynergies.Clear();
        for (int i = 0; i < synergyManager.synergyData.Length; i++)
        {
            var synergy = synergyManager.synergyData[i];
            bool check1 = true; // 해당 시너지를 위한 타워를 모두 쌓았는가?
            bool check2 = false; // 타워 종류가 시너지를 구성하는가?
            foreach (var idxCountPair in synergy.idxCountPairs)
            {
                int towerIdx = idxCountPair.idx;
                int towerCount = idxCountPair.count;
                if (!dict.ContainsKey(towerIdx) || dict[towerIdx] < towerCount) check1 = false;
                if (towerIdx == idx) check2 = true;
            }
            if (check1 && check2) activatedSynergies.Add(i);
        }

        // 스탯 보너스 처리
        radiusBonus = 0;
        delayBonus = 0;
        damageBonus = 0;
        speedBonus = 0;

        foreach (int synergyIdx in activatedSynergies)
        {
            radiusBonus += synergyManager.synergyData[synergyIdx].bonus.radiusBonus;
            delayBonus += synergyManager.synergyData[synergyIdx].bonus.delayBonus;
            damageBonus += synergyManager.synergyData[synergyIdx].bonus.damageBonus;
            speedBonus += synergyManager.synergyData[synergyIdx].bonus.speedBonus;
        }

        UpdateRadiusSphere();
    }
}
