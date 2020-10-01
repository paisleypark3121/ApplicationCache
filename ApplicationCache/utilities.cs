using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ApplicationCache
{
    public class utilities
    {
        public static object ObjectPlusObject(object original, object increment)
        {
            #region try
            try
            {
                #region preconditions
                if ((original == null) && (increment == null))
                    return null;
                if (original == null)
                    return increment;
                if (increment == null)
                    return original;
                #endregion

                if (original.GetType() == typeof(int))
                    return (int)original + (int)increment;
                else if (original.GetType() == typeof(double))
                    return (double)original + (double)increment;
                else if (original.GetType() == typeof(Int16))
                    return (Int16)original + (Int16)increment;
                else if (original.GetType() == typeof(string))
                    return original.ToString() + increment.ToString();

                throw new NotImplementedException();
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                System.Diagnostics.Trace.TraceError(error);
                return null;
            }
            #endregion
        }

        internal static bool ObjectEqualsObject(object value, object limit)
        {
            #region try
            try
            {
                #region preconditions
                if ((value == null) && (limit == null))
                    return true;
                if (value == null)
                    return false;
                if (limit == null)
                    return false;
                #endregion

                if (value.GetType() == typeof(int))
                    return ((int)value == (int)limit);
                else if (value.GetType() == typeof(double))
                    return ((double)value == (double)limit);
                else if (value.GetType() == typeof(Int16))
                    return ((Int16)value == (Int16)limit);
                else if (value.GetType() == typeof(string))
                    return (value.ToString() == limit.ToString());

                throw new NotImplementedException();
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                System.Diagnostics.Trace.TraceError(error);
                return false;
            }
            #endregion
        }
    }
}
