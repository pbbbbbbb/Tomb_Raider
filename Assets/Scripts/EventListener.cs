﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventListener<T>
{
    public delegate void OnValueChangeDelegate(T newVal);
    public event OnValueChangeDelegate OnVariableChange;
    private T m_value;
    public T Value
    {
        get
        {
            return m_value;
        }
        set
        {
            if (m_value.Equals(value) || value.Equals(0)) return;
            OnVariableChange?.Invoke(value);
            m_value = value;
        }
    }
}