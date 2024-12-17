using System;

namespace BussinessAccessLayer
{
    public class EnumCollection
    {
        public enum ActivityType
        {
            Onboard = 0,
            Activate = 1,
            Deactivate = 2,
            Terminate = 3,
            ReEdit = 4,
            ReProcess = 5
        }

        public enum Onboarding
        {
            Pending = 0,
            Approve = 1,
            Decline = 2,
            MakerPending = 3,
            MakerApprove = 4,
            MakerDecline = 5,
            CheckerPending = 6,
            CheckerApprove = 7,
            CheckerDecline = 8,
            AuthorizerPending = 9,
            AuthorizerApprove = 10,
            AuthorizerDecline = 11,
        }

        public enum EnumDBOperationType
        {
            Pending = 0,
            Approve = 1,
            Decline = 2,
            CheckerApprove = 3,
            CheckerDecline = 4,
            AuthApprove = 5,
            AuthDecline = 6,
            Activate = 7,
            Deactivate = 8,
            OnboardTerminate = 9,
            RequestTerminate = 10,
            Reprocess = 11,
            BCApprove = 12,
            BCDecline = 13
        }

        public enum EnumDBOperationTypeOverall
        {
            Pending = 0,
            Approve = 1,
            Decline = 2,
            Terminate = 3,
            Activate = 4,
            Deactivate = 5,
            Onboard = 6,
            ReProcess = 7,
            CheckerApproveDecline = 8,
            MakerApproveDecline = 9,
            AuthApproveDecline = 10
        }

        

        public enum EnumPermissionType
        {
            EnableMakerChecker = 1,
            EnableRoles = 2,
            Export = 3,
            ActiveDeactive = 4,
            ExportGrid = 5,
            PreExport = 6,
            AgL1Export = 7,
            AgL2Export = 8,
            AgL3Export = 9,
            BlockAgentL1 = 10,//New
            BlockAgentL2 = 11,//New
            BlockAgent = 12//New
        }
      
        public enum EnumMenuType
        {
            MenuAdd = 1,
            MenuEdit = 2,
            SubMenuAdd = 3,
            SubMenuEdit = 4,
            MappingPages = 5,
            SwappingPages = 6
        }
        public enum EnumMenuSelectType
        {
            Menu = 1,
            SubMenu = 2
        }

        public enum enumApplicationType
        {
            WEB = 1,
            MOBILE = 2,
            AUTO_SERVICE = 3
        }
        public enum enumChannelType
        {
            AEPS = 1,
            BBPS = 2,
            DMT = 3,
            MATM = 4,
            REFIL = 5,
            IMPS = 6
        }

        public enum EmailConfiguration
        {
            Registration = 1,
            DocumentUpload = 2,
            Verification = 3,
            UpdateID = 4,
            Active = 5,
            Onboard = 6,
            Decline = 7,
            Deactivate = 8,
            Terminated = 9,
            UpdateDetails = 10,
            ResetPassword = 11,
            ChangePassword = 12,
            BlockUserAccount = 13,
            UnblockUserAccount = 14,
            TerminateUserAccount = 15,
            Wallet_Request_Money = 16,
            Wallet_Approved_Request_Money = 17,
            Wallet_Declined_Request_Money = 18,
            Wallet_Bank_Transfer_Request_Money = 19,
            Wallet_Bank_Transfer_Approve_Money = 20,
            Wallet_Bank_Transfer_Decline_Money = 21,
            Wallet_Balance_Credit_Money = 22,
            Wallet_Balance_Commission_Credit = 23,
            Wallet_Balance_Debit_Money = 24,
            Wallet_Balance_Commission_Debit = 25,
            Agent_EKYC = 26,
            ForgetPassword = 27
        }
        public enum AlertType
        {
            SMSAlert = 31,
            EmailAlert = 32,
            NotificationAlert = 33,
        }
       
        public enum ValidateMethodName
        {
            DMTCustomerRegistration = 1,
            DMTRemitterRegistration = 2,
            DMTTransferMoney = 3
        }

        public enum enumControlType
        {
            Page = 1,
            Control = 2,
            Item = 3
        }

        public enum enumOTPType
        {
            ForgotPassword = 1,
            WebLogin = 2,
            BankTransfer = 3
        }
       
        public enum AgentStatus
        {
            Terminated = 3,
            Active = 1,
            InActive = 2

        }

        public enum StageId
        {
            PendingAgentNameVerify = 1,
            FailedAgentNameVerify = 2,

            PendingPincodeVerify = 3,
            FailedPincodeVerify = 4,

            PendingNSVerify = 5,
            FailedNSVerify = 6,

            PendingPANVerify = 7,
            FailedPANVerify = 8,

            PendingIBAVerify = 9, //
            FailedIBAVerify = 10,//

            PendingDocs = 11, //success
            PendingRequestSumbit = 12,

            RequestSumbitted = 13,
            RequestDeleted=14
        }

        public enum StateCityMode
        {
            Country = 1,
            State = 2,
            City = 3,
            Docs = 4,
            All = 5,
            StateCity = 6,
            PinCode = 7,
            ALLStateCity = 8,
            District = 9
        }

        public enum DBFlag
        {
            Insert = 1,
            Update = 2,
            Get = 3,
            Validate = 4
        }

        public enum ActionType
        {
            Onboard = 0,
            Reprocess = 1,
        }

        public enum LimitType
        {
            Channel = 1,
            KYCType = 2,
            DurationType = 3,
            BC = 4,
            Maker = 5,
            Checker = 6,
            Auth = 7,
            SuperAdmin=8
        }
        public enum LimitDBOperationType
        {
            MakerApprove = 1,
            MakerDecline = 2,
            CheckerApprove = 3,
            CheckerDecline = 4,
            AuthApprove = 5,
            AuthDecline = 6,
        }


        public enum EnumFileDesciption
        {
            UploadNegAgentList = 1,
            UploadRestrictedPIN = 2,
            FileDescID = 1,
                UploadBulkAgentManualKYC=10

        }

        //SOR//

        public enum EnumBindingType
        {
            BindGrid = 1,
            Export = 2,
            ExportBulk = 3,
            AgBulkL1 = 4,
            AgBulkL2 = 5,
            AgBulkL3 = 6
        }
        public enum TransactionSource : int
        {
            Others = 0,
            MATM = 1,
            AEPS = 2,
            BBPS = 3,
            DMT = 4,
        }

        public enum EnumRuleType
        {
            BindGrid = 1,
            Export = 2,
            Insert = 3,
            Update = 4,
            Delete = 5,
            Edit = 6,
        }
    }
}
