using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "SO/Stat/Stat")]
public class StatSO : ScriptableObject
{
    private Dictionary<EStatType, StatElement> statDictionary;
    [SerializeField] private List<StatInfo> statInfos;

    private void OnEnable()
    {
        SetupDictionary();
    }

    private void SetupDictionary()
    {
        statDictionary = new Dictionary<EStatType, StatElement>();
        foreach (StatInfo statInfo in statInfos)
        {
            StatElement stat = new StatElement(statInfo.defaultValue);
            statDictionary.Add(statInfo.statType, stat);
        }
    }

    public StatElement GetStatElement(EStatType statType)
    {
        if (statDictionary.ContainsKey(statType))
            return statDictionary[statType];
        else
            return null;
    }

    [ContextMenu("CreateAllStat")]
    public void CreateAllStat()
    {
        foreach (EStatType statType in Enum.GetValues(typeof(EStatType)))
        {
            bool flag = false;
            foreach (var item in statInfos)
            {
                if (item.statType == statType)
                {
                    flag = true;
                    break;
                }
            }
            if (flag) continue;

            StatInfo statInfo = new StatInfo();
            statInfo.statType = statType;
            statInfos.Add(statInfo);
        }
        SetupDictionary();
    }
}
