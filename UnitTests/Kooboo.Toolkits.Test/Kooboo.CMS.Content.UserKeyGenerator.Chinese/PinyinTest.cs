using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Kooboo.CMS.Content.UserKeyGenerator.Chinese;

namespace Kooboo.Toolkits.Test.Kooboo.CMS.Content.UserKeyGenerator.Chinese
{
    [TestClass]
    public class PinyinTest
    {
        [TestMethod]
        public void TestPinyin()
        {
            //中英文混合，带空格（空格会在处理UserKey时被替换，因此这里不处理）
            Assert.AreEqual("Lai-Zi-Kooboo Team-De-Ce-Shi-Title", PinyinConverter.GetPinyin("来自Kooboo Team的测试Title", "-"));

            //英文开头的中英混合
            Assert.AreEqual(PinyinConverter.GetPinyin("Kooboo Team的测试标题", "-"), "Kooboo Team-De-Ce-Shi-Biao-Ti");

            Assert.AreEqual(PinyinConverter.GetPinyin(" "), " ");

            Assert.AreEqual(PinyinConverter.GetPinyin("123456"), "123456");

            Assert.AreEqual(PinyinConverter.GetPinyin(",./!@#"), ",./!@#");

            Assert.AreEqual(PinyinConverter.GetPinyin("abcde"), "abcde");

            //测试分隔符
            Assert.AreEqual(PinyinConverter.GetPinyin("纯中文测试", "_"), "Chun_Zhong_Wen_Ce_Shi");

            Assert.AreEqual(PinyinConverter.GetPinyin("Kooboo Team的测试标题"), "Kooboo TeamDeCeShiBiaoTi");
        }
    }
}
