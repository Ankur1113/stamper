using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Common
{
    public class CommonEnum
    {
        public static class ErrrorMessageEnum
        {
            public const string
                GetSuccessfully = "Get Successfully",
                SavedSuccessfully = "Saved Successfully",
                AddedSuccessfully = "Added Successfully",
                UpdatedSuccessfully = "Updated Successfully",
                DeletedSuccessfully = "Deleted Successfully",
                ActiveSuccessfully = "Active Successfully",
                InactiveSuccessfully = "Inactive Successfully",
                DataNotFound = "Data Not Found",
                DataAlreadyAvailable = "Data Already Available",
                UserPermissionSuccessfully = "Permission Set Successfully",
                FieldIsRequired = "The {0} field is required",
                FileIsRequired = "The File is required",
                UserNamePasswordNotAvailable = "User Name / Password Not Available",
                ChangeAvtarSuccessfully = "Change Avtar Successfully",
                EmailSentSuccessfully = "Email Sent Successfully",
                EmailVerifiedSuccessfully = "Email Verified Successfully",
                SignUpSuccessfully = "SignUp Successfully",
                SignOutSuccessfully = "SignOut Successfully",
                VerifiedLinkSuccessfully = "Verified Link Successfully",
                FailedVerifiedLinkSuccessfully = "Failed Verified Link Successfully",
                UserNotInactiveItself = "User Have Not Permission To InActive/Delete Itself",
                ChangeStorage = "Change Storage Successfully",
                FileToDatabaseMoved = "FileSystem To Database Moved Successfully",
                DatabaseToFileMoved = "Database To FileSystem Moved Successfully",
                MobileOTPSentSuccessfully = "Mobile OTP Sent Successfully",
                EmailOTPSentSuccessfully = "Email OTP Sent Successfully";
        }
    }
}
