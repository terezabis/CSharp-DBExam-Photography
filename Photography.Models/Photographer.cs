using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Photography.Models
{
    public class Photographer
    {
        public Photographer()
        {
            this.Lenses = new HashSet<Len>();
            this.Accessories = new HashSet<Accessory>();
            this.TrainWorkshops = new HashSet<Workshop>();
            this.ParticipWorkshops = new HashSet<Workshop>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Phone]
        public string Phone { get; set; }
        
        public int PrimaryCameraId { get; set; }

        public int SecondaryCameraId { get; set; }


        [ForeignKey("PrimaryCameraId")]
        public virtual Camera PrimaryCamera { get; set; }
        
        [ForeignKey("SecondaryCameraId")]
        public virtual Camera SecondaryCamera { get; set; }

        public virtual ICollection<Len> Lenses { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; }

        [InverseProperty("Trainer")]
        public virtual ICollection<Workshop> TrainWorkshops { get; set; }

        public virtual ICollection<Workshop> ParticipWorkshops { get; set; }

    }

    public class PhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
            string originalValue = (string)value;

            string regularExpressinString = @"\+[0-9]{1,3}\/[0-9]{8,10}";
            Regex regex = new Regex(regularExpressinString);
            if (!regex.IsMatch(originalValue))
            {
                return false;
            }

            return true;
        }
    }
}

