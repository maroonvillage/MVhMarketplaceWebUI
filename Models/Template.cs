using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webcoreapp.Models
{
    [NotMapped]
    public partial class Template
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateMachineName { get; set; }
        public bool? IsMobile { get; set; }
    }
}
