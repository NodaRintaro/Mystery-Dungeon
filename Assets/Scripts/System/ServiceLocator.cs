using System.Collections.Generic;
using System;

/// <summary> サービスロケーター </summary>
public class ServiceLocator
{
    private static readonly Dictionary<Type, object> _serviceDict = new();

    /// <summary> 使用したいサービスクラスを登録する </summary>
    public static void RegisterService<T>(T service)
    {
        if(_serviceDict.ContainsKey(service.GetType()))
            _serviceDict[typeof(T)] = service;
    }

    /// <summary> 目的のサービスが登録済みであればサービスを取得する </summary>
    public static bool TryGetService<T>(out T service)
    {
        // 登録されているサービスの中に目的のサービスがあれば、サービスを取得してtrueを返す
        if (_serviceDict.ContainsKey(typeof(T)))
        {
            service = (T)_serviceDict[typeof(T)];
            return true;
        }

        service = default(T);
        return false;
    }

    /// <summary> サービスクラスの登録を解除する </summary>
    public static void UnregisterService<T>()
    {
        _serviceDict.Remove(typeof(T));
    }
}