using Microsoft.VisualStudio.TestTools.UnitTesting;
using Session1.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Session1.Actions.Tests
{
    [TestClass()]
    public class CreateUserActionTests
    {
        public User User { get; set; }

        [TestInitialize] 
        public void Init()
        {
            User = new User()
            {
                Username = "test123",
                Password = "testtest",
                FullName = "testtest",
                BirthDate = new DateTime(2002, 1, 1),
                FamilyCount = 5,
                Gender = true,
                UserTypeID = 2,
                GUID = Guid.NewGuid(),
            };
        }

        [TestMethod()]
        public void CreateUser()
        {
            var createUserAction = new CreateUserAction();

            var isTrue = createUserAction.CheckIsUsernameTaken(this.User.Username);

            if (isTrue)
            {
                Assert.IsTrue(false, "user name is already exist in the database.");
                return;
            }

            isTrue = createUserAction.CreateUser(User);
            Assert.IsTrue(isTrue, "username is failed to create");
        }

        [TestMethod()]
        public void CheckUsernameIsTaken()
        {
            var createUserAction = new CreateUserAction();

            var isTrue = createUserAction.CheckIsUsernameTaken("sirvard");

            Assert.IsTrue(isTrue, "username is still available");    
        }

        [TestCleanup]
        public void Cleanup()
        {
            var createUserAction = new CreateUserAction();
            createUserAction.RemoveUserByUsername(this.User.Username);  
        }
    }
}