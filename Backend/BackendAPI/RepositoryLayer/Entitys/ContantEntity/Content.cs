using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entitys.ContantEntity
{
    public class Content
    {
            [Key]
            public int ContentId { get; set; }

            public int NumberOfViews { get; set; }

            [Required, MaxLength(100)]
            public string Category { get; set; }

            [Required, MaxLength(200)]
            public string Title { get; set; }

            [Required, EnumDataType(typeof(ContentStatus))]
            public ContentStatus Status { get; set; }

            [Required, EnumDataType(typeof(ContentType))]
            public ContentType TypeOfContent { get; set; }
            
            public DateTime CreatedAt { get; set; }

            public bool IsActive { get; set; }=false;

        
    }
}
