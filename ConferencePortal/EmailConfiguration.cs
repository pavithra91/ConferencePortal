//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConferencePortal
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailConfiguration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailConfiguration()
        {
            this.Configurations = new HashSet<Configuration>();
        }
    
        public int AUTOID { get; set; }
        public Nullable<int> ConventionID { get; set; }
        public string EmailTemplate { get; set; }
        public string FromEmail { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
    
        public virtual Configuration Configuration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration> Configurations { get; set; }
    }
}