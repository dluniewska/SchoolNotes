using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public partial class FileData
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public string? UploadedBy { get; set; }
        //public virtual User? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string fileUploadName { get; set; }
        public byte[] FileUpload { get; set; }

    }
} 
 