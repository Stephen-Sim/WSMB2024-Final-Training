using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sessio4Test;
using System;
using System.Linq;

namespace Session4Test
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// availabilities before 19/02/2017
        /// </summary>
        [TestMethod]
        public void CheckItemPriceDate()
        {
            using (var ent = new WSC2022SE_Session4Entities())
            {
                var isany = ent.ItemPrices.Any(x => x.Date < new DateTime(2017, 2, 17));
                Assert.IsFalse(isany, "there availabilities before 19/02/2017");
            }
        }

        /// <summary>
        /// No score associated with any of the properties or listings should 
        /// have higher score than 5 and lower score than 0.
        /// </summary>
        [TestMethod]
        public void CheckItemScore()
        {
            using (var ent = new WSC2022SE_Session4Entities())
            {
                var isany = ent.ItemScores.Any(x => x.Value > 5 || x.Value < 0);
                Assert.IsFalse(isany, "hvaing properties or listings " +
                    "have score than 5 and lower score than 0.");
            }
        }

        /// <summary>
        /// can never have duplicate users registered.
        /// </summary>
        [TestMethod]
        public void CheckDuplicatedUser()
        {
            using (var ent = new WSC2022SE_Session4Entities())
            {
                var isany = ent.Users.GroupBy(x => new
                {
                    x.Username
                }).Any(x => x.Count() >= 2);

                Assert.IsFalse(isany, "system have duplicate users registered");
            }
        }

    }
}
