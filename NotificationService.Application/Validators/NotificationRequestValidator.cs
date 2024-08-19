using System.Data;
using FluentValidation;
using NotificationService.Commons.Request;

namespace NotificationService.Application.Validators;

public class NotificationRequestValidator : AbstractValidator<NotificationRequest>
{
    public NotificationRequestValidator()
    {
        RuleFor(x=> x.Recipient.EmailAdress).NotEmpty().MaximumLength(100).EmailAddress().WithMessage("Email not in expected format");

        RuleFor(x => x.Recipient.Name).NotEmpty().MaximumLength(100).WithMessage("Name cannot be empty");

        RuleFor(x => x.Message.Body).NotEmpty().MaximumLength(200).WithMessage("Message Body cannot be empty");
        
        RuleFor(x => x.Message.Title).NotEmpty().MaximumLength(50).WithMessage("Message Title cannot be empty");
        
    }
}