using FluentValidation.Results;

namespace Prospecto.Models.DTO.FluentValidation
{
    public abstract class DTOValidatorBase
    {
        public virtual void BeforeValidate()
        { }

        public abstract ValidationResult Validate();
    }
}
