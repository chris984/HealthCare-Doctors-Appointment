//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HealthCare.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdminTable
    {
        public int Admin_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public string Name { get; set; }
        public string Hospital_Name { get; set; }
        public string Hospital_Address { get; set; }
        public string Hospital_Contact_No { get; set; }
    
        public virtual User_Table User_Table { get; set; }
    }
}
