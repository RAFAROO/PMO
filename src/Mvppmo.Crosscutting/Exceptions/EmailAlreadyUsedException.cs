using Mvppmo.Crosscutting.Constants;

namespace Mvppmo.Crosscutting.Exceptions;

public class EmailAlreadyUsedException : BadRequestAlertException
{
    public EmailAlreadyUsedException() : base(ErrorConstants.EmailAlreadyUsedType, "Email is already in use!",
        "userManagement", "emailexists")
    {
    }
}
