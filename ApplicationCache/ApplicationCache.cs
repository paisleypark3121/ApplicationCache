using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

namespace ApplicationCache
{
    public class ApplicationCache : ICache
    {
        #region singleton
        private static ApplicationCache _instance = new ApplicationCache();

        public static ApplicationCache Instance
        {
            get { return _instance; }
        }
        #endregion

        private MemoryCache _cache = null;

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
            #region try
            try
            {
                if (_cache == null)
                {
                    string _cache_info = "NULL CACHE";
                    Guid _guid = Guid.NewGuid();
                    _cache = new MemoryCache(_guid.ToString());
                    _cache_info += ": " + _guid.ToString() + "\n";
                }

                DateTimeOffset _local_offset = (_expiration_in_minute == -1) ? DateTimeOffset.Now.AddMinutes(expiration) : DateTimeOffset.Now.AddMinutes(_expiration_in_minute);
                _cache.Set(key, value, _local_offset);

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
            #region try
            try
            {
                #region precondition
                if (_cache == null)
                    return true;
                #endregion

                object result = _cache.Remove(key);

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
        }
        #endregion
    }
}
