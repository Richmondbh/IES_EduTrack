#nullable disable
using System;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Abstract base class representing any person in the system.
    /// Student and Staff both inherit from this class.
    /// </summary>
    public abstract class Person
    {
        private string _personId;
        private string _name;
        private string _email;
        private DateTime _dateOfBirth;

        public string PersonId
        {
            get { return _personId; }
            set
            {
                // Guard against null or empty id
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PersonId cannot be empty.");
                _personId = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        // Returns display-friendly full name — used in ListBox bindings
        public virtual string GetDisplayName()
        {
            return _name;
        }

        public override string ToString()
        {
            return $"{_name} ({_personId})";
        }
    }
}