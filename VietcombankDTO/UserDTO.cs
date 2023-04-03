using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietcombankDTO
{
    public class UserDTO
    {
        private string? _iD;
        private string? _firstName;
        private string? _lastName;
        private string? _dateOfBirth;
        private string? _email;
        private string? _phone;
        private string? _address;
        private string? _numberAccount;
        private decimal _accountBalance;
        private string? _username;
        private string? _role;

        public string? ID { get => _iD; set => _iD = value; }
        public string? FirstName { get => _firstName; set => _firstName = value; }
        public string? LastName { get => _lastName; set => _lastName = value; }
        public string? DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public string? Email { get => _email; set => _email = value; }
        public string? Phone { get => _phone; set => _phone = value; }
        public string? Address { get => _address; set => _address = value; }
        public string? NumberAccount { get => _numberAccount; set => _numberAccount = value; }
        public decimal AccountBalance { get => _accountBalance; set => _accountBalance = value; }
        public string? Username { get => _username; set => _username = value; }
        public string? Role { get => _role; set => _role = value; }
    }
}
