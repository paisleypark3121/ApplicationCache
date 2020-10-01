using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationCache.Test
{
    [TestClass]
    public class ApplicationCacheTest
    {
        [TestMethod]
        public void SetGet_1_ok()
        {
            #region arrange
            bool expected_set = true;
            object expected_get = 1;
            #endregion

            #region act
            bool actual_set = ApplicationCache.Instance.set("click", 1);
            object actual_get = ApplicationCache.Instance.get("click");
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
            ParallelLoopResult result = Parallel.For(0, 10, (i) => ApplicationCache.Instance.set("click", i));
            bool actual_isCompleted = result.IsCompleted;
            object actual_get = ApplicationCache.Instance.get("click");
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
            ApplicationCache.Instance.set("click", 0);
            #endregion

            #region act
            ParallelLoopResult result = Parallel.For(0, 10, (i) =>
            {
                object clickObj = ApplicationCache.Instance.get("click");
                int click = (int)clickObj + 1;
                ApplicationCache.Instance.set("click", click);
            }
            );
            bool actual_isCompleted = result.IsCompleted;
            object actual_get = ApplicationCache.Instance.get("click");
            #endregion

            #region assert
            Assert.AreEqual(expected_isCompleted, actual_isCompleted);
            #endregion
        }

    }
}
