using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
    public class Tests {

        [TestMethod]
        public void True() {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void False() {
            Assert.IsFalse(false);
        }

    }
}
