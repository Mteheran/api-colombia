
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public enum SortDirection
    { 
        [Display(Name = "asc")]
        Asc,

        [Display(Name = "desc")]
        Desc
    }