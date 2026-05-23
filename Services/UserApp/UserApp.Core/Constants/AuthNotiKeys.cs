using FollowMe.Library.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Constants
{

    public static class AuthNotiKeys
    {
        // Nhóm lỗi liên quan đến Đăng ký & Tài khoản
        public static readonly ModelNoti NameInvalid = new("AUTH_NAME_INVALID", "auth.error.name_invalid");
        public static readonly ModelNoti EmailDuplicated = new("AUTH_EMAIL_DUPLICATED", "auth.error.email_duplicated");
        public static readonly ModelNoti UsernameDuplicated = new("AUTH_USERNAME_DUPLICATED", "auth.error.username_duplicated");
        public static readonly ModelNoti RegisterFailed = new("AUTH_REGISTER_FAILED", "auth.error.register_failed");

        // Nhóm trạng thái liên quan đến Đăng nhập / Xác thực
        public static readonly ModelNoti CredentialsValidated = new("AUTH_CREDENTIALS_VALIDATED", "auth.success.credentials_validated");
        public static readonly ModelNoti InvalidCredentials = new("AUTH_INVALID_CREDENTIALS", "auth.error.invalid_credentials");
        public static readonly ModelNoti AccountLocked = new("AUTH_ACCOUNT_LOCKED", "auth.error.account_locked");
        public static readonly ModelNoti UserNotFound = new("AUTH_USER_NOT_FOUND", "auth.error.user_not_found");
        public static readonly ModelNoti InvalidPassword = new("AUTH_INVALID_PASSWORD", "auth.error.invalid_password");

        // Nhóm lỗi hệ thống & phân vùng hạ tầng
        public static readonly ModelNoti RegionNotFound = new("REGION_NOT_FOUND", "common.error.region_not_found");
        public static readonly ModelNoti InternalServerError = new("INTERNAL_SERVER_ERROR", "common.system.internal_error");
    }

}
