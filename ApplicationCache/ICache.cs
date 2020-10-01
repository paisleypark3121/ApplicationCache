using System;

namespace ApplicationCache
{
    public interface ICache
    {
        bool set(string key, object value, int _expiration_in_minutes = -1);
        object get(string key);

        bool remove(string key);
    }
}
