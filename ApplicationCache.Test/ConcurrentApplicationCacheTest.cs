using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationCache.Test
{
    [TestClass]
    public class ConcurrentApplicationCacheTest
    {
        [TestMethod]
        public void GetSet()
        {
            #region arrange
            bool expected_set = true;
            object expected_get = 1;
            #endregion

            #region act
            bool actual_set = ConcurrentApplicationCache.Instance.set("click", 1);
            object actual_get = ConcurrentApplicationCache.Instance.get("click");
            #endregion

            #region assert
            Assert.AreEqual(expected_set, actual_set);
            Assert.AreEqual(expected_get, actual_get);
            #endregion
        }

        [TestMethod]
        public void MultipleSetGet_ok()
        {
            #region arrange
            bool expected_isCompleted = true;
            #endregion

            #region act
            ParallelLoopResult result = Parallel.For(0, 10, (i) => ConcurrentApplicationCache.Instance.set("click", i));
            bool actual_isCompleted = result.IsCompleted;
            object actual_get = ConcurrentApplicationCache.Instance.get("click");
            #endregion

            #region assert
            Assert.AreEqual(expected_isCompleted, actual_isCompleted);
            #endregion
        }

        [TestMethod]
        public void MultipleIncrease_ok()
        {
            #region arrange
            bool expected_isCompleted = true;
            ConcurrentApplicationCache.Instance.set("click", 0);
            int concurrentProcesses = 10;
            #endregion

            #region act
            ParallelLoopResult result = Parallel.For(0, concurrentProcesses, (i) =>
                ConcurrentApplicationCache.Instance.setGetPlusValue("click", 1)
            );
            bool actual_isCompleted = result.IsCompleted;
            object actual_get = ConcurrentApplicationCache.Instance.get("click");
            #endregion

            #region assert
            Assert.AreEqual(expected_isCompleted, actual_isCompleted);
            Assert.AreEqual(concurrentProcesses, actual_get);
            #endregion
        }

        [TestMethod]
        public void MultipleIncreaseLimit_ok()
        {
            #region arrange
            bool expected_isCompleted = true;
            ConcurrentApplicationCache.Instance.set("click", 0);
            int concurrentProcesses = 10;
            int limit = 8;
            int reset = 0;
            List<int> clicks = new List<int>();
            #endregion

            #region act
            ParallelLoopResult result = Parallel.For(0, concurrentProcesses, (i) =>
            {
                object result_set = ConcurrentApplicationCache.Instance.setGetPlusValueLimit("click", 1, limit, reset);
                if (result_set != null)
                    clicks.Add((int)result_set);
            }
            );
            bool actual_isCompleted = result.IsCompleted;
            int peek = clicks[concurrentProcesses - 1];
            #endregion

            #region assert
            Assert.AreEqual(expected_isCompleted, actual_isCompleted);
            Assert.AreEqual(concurrentProcesses - limit, peek);
            #endregion
        }
    }
}
