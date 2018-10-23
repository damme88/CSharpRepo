using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectData
{
    public class AccountInfo
    {
        private Int32 _id;
        private string _address;
        private string _nickName;
        private string _email;
        private string _password;
        private string _description;

        public AccountInfo()
        {
            _id = 0;
            _address = string.Empty;
            _nickName = string.Empty;
            _email = string.Empty;
            _password = string.Empty;
            _description = string.Empty;
        }

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string NickName
        {
            set { _nickName = value; }
            get { return _nickName; }
        }

        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }

        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }

        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
    }
}
