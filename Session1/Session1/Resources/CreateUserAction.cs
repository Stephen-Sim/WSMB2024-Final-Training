using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Session1.Actions
{
    public class CreateUserAction
    {
        public bool CheckIsUsernameTaken(string username)
        {
            using (var ent = new WSC2022SE_Session1Entities())
            {
                if (ent.Users.Any(x => x.Username == username))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CreateUser(User user)
        {
            try
            {
                using (WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities())
                {
                    ent.Users.Add(user);
                    ent.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveUserByUsername(string username)
        {
            using (var ent = new WSC2022SE_Session1Entities())
            {
                var user = ent.Users.FirstOrDefault(x => x.Username == username);

                if (user == null)
                {
                    return;
                }

                ent.Users.Remove(user);
                ent.SaveChanges();
            }
        }
    }
}
