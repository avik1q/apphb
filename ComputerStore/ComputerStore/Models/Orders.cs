using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerStore.Models
{
    public class Orders
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "you must enter product name")]
        public string Name { get; set; }
        [RegularExpression("^[1-9][0-9]?$", ErrorMessage = " נא להזין מספר שלם גדול מ-0 וקטן מ-100")]
        [Range(1, 99, ErrorMessage = "אין אפשרות למספר שלילי או גדול מ -99")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "you must enter valid date")]
        public DateTime Date { get; set; }
        [RegularExpression("[^0-9]*", ErrorMessage = "נא להזין שם חוקי באנגלית או בעברית ללא מספרים")]
        [Required(ErrorMessage = "נא להזין שם קונה")]
        public string BuyerName { get; set; }
    }
}