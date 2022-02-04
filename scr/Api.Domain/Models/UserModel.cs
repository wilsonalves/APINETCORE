using System;

namespace Api.Domain.Models
{
    public class UserModel
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        private DateTime _createAt;
        public DateTime createAt
        {
            get { return _createAt; }
            set
            {
                _createAt = value == null ? DateTime.UtcNow : value;
            }
        }


    }
}