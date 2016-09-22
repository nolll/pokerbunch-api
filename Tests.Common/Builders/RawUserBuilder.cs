using Infrastructure.Data.Classes;

namespace Tests.Common.Builders
{
    public class RawUserBuilder
    {
        private int _id;
        private string _userName;
        private string _displayName;
        private string _realName;
        private string _email;
        private int _globalRole;
        private string _encryptedPassword;
        private string _salt;

        public RawUserBuilder()
        {
            _id = 0;
            _userName = "";
            _displayName = "";
            _realName = "";
            _email = "";
            _globalRole = 0;
            _encryptedPassword = "";
            _salt = "";
        }

        public RawUser Build()
        {
            return new RawUser(_id, _userName, _displayName, _realName, _email, _globalRole, _encryptedPassword, _salt);
        }

        public RawUserBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public RawUserBuilder WithUserName(string userName)
        {
            _userName = userName;
            return this;
        }
        
        public RawUserBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }
        
        public RawUserBuilder WithRealName(string realName)
        {
            _realName = realName;
            return this;
        }
        
        public RawUserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public RawUserBuilder WithGlobalRole(int globalRole)
        {
            _globalRole = globalRole;
            return this;
        }

        public RawUserBuilder WithEncryptedPassword(string encryptedPassword)
        {
            _encryptedPassword = encryptedPassword;
            return this;
        }

        public RawUserBuilder WithSalt(string salt)
        {
            _salt = salt;
            return this;
        }
    }
}