using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerStore.Models
{
    public class Inventory
    {
        [Key]
        [Required(ErrorMessage ="נא למלא שדה הברקוד")]
        public string BarCode { get; set; }
        [Required(ErrorMessage = "נא למלא שדה שם המוצר")]
        public string Name { get; set; }
        [Required(ErrorMessage = "נא למלא שדה הכמות")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "נא להזין מספר שלם חיובי")]
        public int Amount { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "נא להזין מספר שלם חיובי")]
        [Required(ErrorMessage = "נא למלא שדה המחיר")]
   //   [Range(0, 99999999999999999, ErrorMessage = "המספר לא יכול להיות שלילי או גדול מאוד")] // regular expression is better
        public int Price { get; set; }
        [Required(ErrorMessage = "נא למלא שדה התמונה")]
        public string url { get; set; }
    }
}