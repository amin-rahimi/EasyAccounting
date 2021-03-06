//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyAccounting.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Photos = new HashSet<Photo>();
            this.Contracts = new HashSet<Contract>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedDate { get; set; }
    
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
