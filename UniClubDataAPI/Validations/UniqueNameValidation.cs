using System.ComponentModel.DataAnnotations;
using System.Reflection;
using UniClubDataAPI.Data;

namespace UniClubDataAPI.Validations
{
    public class UniqueNameValidation : ValidationAttribute
    {
        private readonly string _universityId;

        public UniqueNameValidation(string universityId)
        {
            _universityId = universityId;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var uniIdProp = validationContext.ObjectType.GetProperty(_universityId);

            if(uniIdProp == null)
            {
                return new ValidationResult(String.Format("Unknown property {0}", _universityId));
            }

            int uniId = (int) uniIdProp.GetValue(validationContext.ObjectInstance, null);
            ApplicationDBContext db = validationContext.GetService(typeof(ApplicationDBContext)) as ApplicationDBContext;

            if(db == null)
            {
                return new ValidationResult("Could not connect to DB");
            }
            
            if(db.Clubs.Any(c => c.UniversityId == uniId && object.Equals(c.Name, value)))
            {
                return new ValidationResult("Club already exists");
            }
            return ValidationResult.Success;
        }
    }
}
