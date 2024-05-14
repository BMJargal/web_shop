 using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;

namespace Web_Shop.Application.Validation
{
    public class AddUpdateProductDTOValidator : AbstractValidator<AddUpdateProductDTO>
    {
        public AddUpdateProductDTOValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(request => request.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(request => request.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(request => request.Sku).NotEmpty().WithMessage("SKU is required.");
        }
    }
}
