using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThreadUtility : MonoBehaviour
{
    private static MainThreadUtility m_Instance = null;
    private static readonly Queue<Action> m_Queue = new();
    private static Thread m_MainThread;

    [RuntimeInitializeOnLoadMethod]
    private static MainThreadUtility GetInstance()
    {
        if (m_Instance == null)
        {
            var g = new GameObject("MainThreadUtility");
            m_Instance = g.AddComponent<MainThreadUtility>();
            DontDestroyOnLoad(g);
        }

        return m_Instance;
    }

    private void Awake() => m_MainThread = Thread.CurrentThread;

    private void OnDestroy() => m_Instance = null;

    private void Update()
    {
        lock (m_Queue)
        {
            while (m_Queue.Count > 0)
                m_Queue.Dequeue().Invoke();
        }
    }

    private void FixedUpdate()
    {
        lock (m_Queue)
        {
            while (m_Queue.Count > 0)
                m_Queue.Dequeue().Invoke();
        }
    }

    public static void Execute(Action action)
    {
        lock (m_Queue)
        {
            if (m_MainThread != null && m_MainThread == Thread.CurrentThread)
                action.Invoke();
            else
                m_Queue.Enqueue(action);
        }
    }
}
