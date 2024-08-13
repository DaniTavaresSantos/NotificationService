namespace NotificationService.Infra.Cache.Abstractions;

public interface ICacheRepository

{
    /// <summary>
    /// Get Data using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T GetData<T>(string key);

    /// <summary>
    /// Set data with value and expiration time of key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expirationTime"></param>
    /// <returns></returns>
    void SetData<T>(string key, T value, TimeSpan expirationTime);
}