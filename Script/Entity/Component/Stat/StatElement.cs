using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatElement
{
    [SerializeField] private float _baseValue;
    [SerializeField] private List<float> _addModifies = new List<float>();
    [SerializeField] private List<float> _percentModifies = new List<float>();

    public float Value { get; private set; }

    public StatElement(float baseValue)
    {
        _baseValue = baseValue;
        SetValue();
    }

    private void SetValue()
    {
        //덧셈 변경사항 적용
        float numValue = _baseValue;
        for (int i = 0; i < _addModifies.Count; i++)
        {
            numValue += _addModifies[i];
        }

        //퍼센트 변경사항 적용
        float percentModify = 0;
        for (int i = 0; i < _percentModifies.Count; i++)
        {
            percentModify += _percentModifies[i];
        }

        //덧셈 변경 적용 후 퍼센트 변경 적용
        float value = numValue * (1 + percentModify / 100);

        Value = value;
    }

    public void AddModify(float modify, bool _isPercentModify)
    {
        if (_isPercentModify)
            _percentModifies.Add(modify);
        else
            _addModifies.Add(modify);
        SetValue();
    }
    public void RemoveModify(float modify, bool _isPercentModify)
    {
        if (_isPercentModify && _percentModifies.Contains(modify))
            _percentModifies.Add(modify);
        else if (_addModifies.Contains(modify))
            _addModifies.Add(modify);
        SetValue();
    }
}
