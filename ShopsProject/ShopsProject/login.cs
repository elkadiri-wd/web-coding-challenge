//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopsProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class login
    {
        public login()
        {
            this.UserShop = new HashSet<UserShop>();
        }
    
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    
        public virtual ICollection<UserShop> UserShop { get; set; }
    }
}
