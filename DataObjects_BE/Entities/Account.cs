using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Role {  get; set; }
        public DateTime DateCreateAt { get; set; }
        public DateTime? DateUpdateAt { get; set;}
        public string ImageUrl { get; set; }


        public virtual Manager Manager { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
