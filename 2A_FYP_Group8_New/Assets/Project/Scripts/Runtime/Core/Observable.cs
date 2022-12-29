using System;
using System.Linq;
using System.Collections.Generic;

public interface IObservable<T>
{
    public T Value { get; }

    public void SetValue(T value);
    public void SetValueSilent(T value, Action<T> silentSubscriber);

    public void SubscribeImmediate(Action<T> action, bool invokeActionOnSubscribe = true);
    public void Subscribe(Action<T> action, bool invokeActionOnSubscribe = true);
    public void Unsubscribe(Action<T> action);
}

public class Observable<T> : IObservable<T>
{
    #region Properties
    protected T m_Value;
    public T Value => m_Value;

    private readonly List<Action<T>> m_SubscribersImmediate = new();
    private readonly List<Action<T>> m_Subscribers = new();
    private readonly Dictionary<object, Action<T>> m_Callbacks = new();
    private Action[] m_Actions = new Action[10]; // Action pool for main thread execution
    #endregion

    // IObservable
    public void SetValue(T obj)
        => SetValue(obj, null);

    public void SetValueSilent(T obj, Action<T> silentSubscriber)
        => SetValue(obj, silentSubscriber);

    public void SubscribeImmediate(Action<object> action, bool invokeActionOnSubscribe = true)
    {
        void a(T obj) => action(obj);
        m_Callbacks.Add(action, a);
        SubscribeImmediate(a, invokeActionOnSubscribe);
    }

    public void Subscribe(Action<object> action, bool invokeActionOnSubscribe = true)
    {
        void a(T obj) => action(obj);
        m_Callbacks.Add(action, a);
        Subscribe(a, invokeActionOnSubscribe);
    }

    public void Unsubscribe(Action<object> action)
    {
        if (!m_Callbacks.ContainsKey(action))
            return;

        Unsubscribe(m_Callbacks[action]);
        m_Callbacks.Remove(action);
    }

    // Observable
    protected virtual void SetValue(T value, Action<T> silentSubscriber = null)
    {
        if (m_Value.Equals(value))
            return;

        m_Value = value;
        Trigger(value, silentSubscriber);
    }

    public void SubscribeImmediate(Action<T> action, bool invokeActionOnSubscribe = true)
    {
        m_SubscribersImmediate.Add(action);
        if (invokeActionOnSubscribe)
            action(m_Value);
    }

    public void Subscribe(Action<T> action, bool invokeActionOnSubscribe = true)
    {
        m_Subscribers.Add(action);
        if (invokeActionOnSubscribe)
            action(m_Value);
    }

    public void Unsubscribe(Action<T> action)
    {
        if (m_SubscribersImmediate.Contains(action))
            m_SubscribersImmediate.Remove(action);

        if (m_Subscribers.Contains(action))
            m_Subscribers.Remove(action);
    }

    private int NotUsedActionPlaceholder()
    {
        for (int i = 0; i < m_Actions.Length; ++i)
        {
            if (m_Actions[i] == null)
                return i;
        }

        // Extend array to register the latest data manipulation
        Array.Resize(ref m_Actions, m_Actions.Length + 1);
        return m_Actions.Length - 1;
    }

    protected virtual void Trigger(T value, Action<T> silentSubscriber = null)
    {
        List<Action<T>> subscribersImmediateList;
        if (silentSubscriber == null || !m_Callbacks.ContainsKey(silentSubscriber))
            subscribersImmediateList = m_SubscribersImmediate;
        else
            subscribersImmediateList = m_SubscribersImmediate.Where(s => s != m_Callbacks[silentSubscriber]).ToList();

        foreach (var subscriber in subscribersImmediateList)
            subscriber(value);

        // Send actions to main thread
        List<Action<T>> subscribersList;
        if (silentSubscriber == null || !m_Callbacks.ContainsKey(silentSubscriber))
            subscribersList = m_Subscribers;
        else
            subscribersList = m_Subscribers.Where(s => s != m_Callbacks[silentSubscriber]).ToList();

        int index = NotUsedActionPlaceholder();
        m_Actions[index] = () =>
        {
            foreach (var subscriber in subscribersList)
                subscriber(value);

            m_Actions[index] = null;
        };

        MainThreadUtility.Execute(m_Actions[index]);
    }
}
