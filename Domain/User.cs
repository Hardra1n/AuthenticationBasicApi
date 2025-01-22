using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        private string _firstName;
        private string _lastName;
        private DateOnly birthday;

        public Guid Id { get; set; }
        public string FirstName
        {
            get => _firstName;
            set
            {
                
                _firstName = ValidateName(value);
            }
        }

        public string LastName
        {
            get => _lastName; 
            set
            {
                _lastName = ValidateName(value);
            }
        }
        public DateOnly Birthday
        {
            get => birthday;
            set
            {
                var minDate = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * 100));
                var today = DateOnly.FromDateTime(DateTime.Now);
                if (value > today || value < minDate)
                {
                    throw new InnerLogicExeption($"Birthday must be between '{minDate}' and '{today}, value: {value}'");
                }
                birthday = value;
            }
        }



        private string ValidateName(string name)
        {
            string value = name.Trim();
            StringBuilder valueWithoutMultipleSpaces = new();
            bool spaceFlag = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]) && spaceFlag)
                {
                    continue;
                }
                valueWithoutMultipleSpaces.Append(value[i]);

                if (char.IsWhiteSpace(value[i]))
                {
                    spaceFlag = true;
                }
                else
                {
                    spaceFlag = false;
                }
            }

            value = valueWithoutMultipleSpaces.ToString();

            if (!Regex.IsMatch(name, "^.{1,25}$"))
            {
                throw new InnerLogicExeption("Name length must be less then 25 and not empty");
            }
            if (!Regex.IsMatch(name, "^[A-Za-z\\s]*$"))
            {
                throw new InnerLogicExeption("Name can contain only latin symbols and spaces");
            }

            return value;
        }

        public User DeepCopy()
        {
            User user = (User)this.MemberwiseClone();
            return user;
        }
    }
}
