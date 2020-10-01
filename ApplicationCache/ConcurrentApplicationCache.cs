using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace ApplicationCache
{
    public class ConcurrentApplicationCache : ICache
    {
        #region singleton
        private static ConcurrentApplicationCache _instance = new ConcurrentApplicationCache();

        public static ConcurrentApplicationCache Instance
        {
            get { return _instance; }
        }
        #endregion

        private MemoryCache _cache = null;
        private Mutex _mutex = null;

        #region expiration
        // Expiration in minutes
        private int _expiration = 240;
        public int expiration
        {
            get
            {
                return _expiration;
            }
            set
            {
                _expiration = value;
            }
        }
        #endregion

        #region ICache
        public bool set(string key, object value, int _expiration_in_minute = -1)
        {
            #region variables
            bool isOwnTheMutex = false;
            #endregion
            #region try
            try
            {
                #region MUTEX
                if (_mutex == null)
                    _mutex = new Mutex();
                #endregion

                #region MUTEX on
                isOwnTheMutex = _mutex.WaitOne();
                #endregion

                #region null cache
                if (_cache == null)
                {
                    string _cache_info = "NULL CACHE";
                    Guid _guid = Guid.NewGuid();
                    _cache = new MemoryCache(_guid.ToString());
                    _cache_info += ": " + _guid.ToString() + "\n";
                }
                #endregion

                #region manage key
                DateTimeOffset _local_offset = (_expiration_in_minute == -1) ? DateTimeOffset.Now.AddMinutes(expiration) : DateTimeOffset.Now.AddMinutes(_expiration_in_minute);
                _cache.Set(key, value, _local_offset);
                #endregion

                return true;
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                Trace.TraceError(error);
                return false;
            }
            #endregion
            #region finally
            finally
            {
                if ((_mutex != null) && (isOwnTheMutex))
                    _mutex.ReleaseMutex();
            }
            #endregion
        }

        public object get(string key)
        {
            #region try
            try
            {
                #region precondition
                if (_cache == null)
                    return null;
                #endregion

                object result = _cache.Get(key);

                return result;
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                Trace.TraceError(error);
                return false;
            }
            #endregion
        }

        public bool remove(string key)
        {
            #region variables
            bool isOwnTheMutex = false;
            #endregion
            #region try
            try
            {
                #region precondition
                if (_cache == null)
                    return true;
                #endregion

                #region MUTEX on
                isOwnTheMutex = _mutex.WaitOne();
                #endregion

                _cache.Remove(key);

                return true;
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                Trace.TraceError(error);
                return false;
            }
            #endregion
            #region finally
            finally
            {
                if ((_mutex != null) && (isOwnTheMutex))
                    _mutex.ReleaseMutex();
            }
            #endregion
        }

        public object setGetPlusValue(string key, object value, int _expiration_in_minute = -1)
        {
            #region variables
            bool isOwnTheMutex = false;
            #endregion
            #region try
            try
            {
                #region MUTEX
                if (_mutex == null)
                    _mutex = new Mutex();
                #endregion

                #region MUTEX on
                isOwnTheMutex = _mutex.WaitOne();
                #endregion

                #region null cache
                if (_cache == null)
                {
                    Guid _guid = Guid.NewGuid();
                    _cache = new MemoryCache(_guid.ToString());                    
                }
                #endregion

                #region get value
                object result = _cache.Get(key);
                if (result != null)
                    value = utilities.ObjectPlusObject(result, value);
                #endregion

                #region manage key
                DateTimeOffset _local_offset = (_expiration_in_minute == -1) ? DateTimeOffset.Now.AddMinutes(expiration) : DateTimeOffset.Now.AddMinutes(_expiration_in_minute);
                _cache.Set(key, value, _local_offset);
                #endregion

                return true;
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                Trace.TraceError(error);
                return false;
            }
            #endregion
            #region finally
            finally
            {
                if ((_mutex != null) && (isOwnTheMutex))
                    _mutex.ReleaseMutex();
            }
            #endregion
        }

        /// <summary>
        /// Retrieves the value of the "key" and adds "value" to it;
        /// in case the new value reaches "limit" it is set to "reset"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="limit"></param>
        /// <param name="reset"></param>
        /// <param name="_expiration_in_minute"></param>
        /// <returns></returns>
        public object setGetPlusValueLimit(
            string key,
            object value,
            object limit,
            object reset,
            int _expiration_in_minute = -1)
        {
            #region variables
            bool isOwnTheMutex = false;
            #endregion
            #region try
            try
            {
                #region MUTEX
                if (_mutex == null)
                    _mutex = new Mutex();
                #endregion

                #region MUTEX on
                isOwnTheMutex = _mutex.WaitOne();
                #endregion

                #region null cache
                if (_cache == null)
                {
                    string _cache_info = "NULL CACHE";
                    Guid _guid = Guid.NewGuid();
                    _cache = new MemoryCache(_guid.ToString());
                    _cache_info += ": " + _guid.ToString() + "\n";
                    
                }
                #endregion

                #region get value
                object result = _cache.Get(key);
                if (result != null)
                    value = utilities.ObjectPlusObject(result, value);
                if (utilities.ObjectEqualsObject(value, limit))
                    value = reset;
                #endregion

                #region manage key
                DateTimeOffset _local_offset = (_expiration_in_minute == -1) ? DateTimeOffset.Now.AddMinutes(expiration) : DateTimeOffset.Now.AddMinutes(_expiration_in_minute);
                _cache.Set(key, value, _local_offset);
                #endregion

                return value;
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                Trace.TraceError(error);
                return null;
            }
            #endregion
            #region finally
            finally
            {
                if ((_mutex != null) && (isOwnTheMutex))
                    _mutex.ReleaseMutex();
            }
            #endregion
        }
        #endregion
    }
}
