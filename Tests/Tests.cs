using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
    public class Tests {

        [TestMethod, TestCategory("Running")]
        public void True() {
            Assert.IsTrue(true);
        }

        [TestMethod, TestCategory("Running")]
        public void False() {
            Assert.IsFalse(false);
        }

    }
}
