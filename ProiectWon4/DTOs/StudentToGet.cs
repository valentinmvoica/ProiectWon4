﻿namespace ProiectWon4.DTOs
{
    public class StudentWithAddressToGet
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public AddressToGet Address { get; set; }
    }

}
