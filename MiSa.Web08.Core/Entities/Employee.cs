using MiSa.Web08.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiSa.Web08.Core
{
    public class Employee
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }


        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string EmployeeCode { get; set; }


        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [NotEmpty]
        public string EmployeeName { get; set; }


        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Date]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        [NotMap]
        public string? GenderName
        {
            get
            {
                switch (Gender)
                {
                    case Enum.Gender.Male:
                        return Properties.Resource.Enum_Gender_Male;
                    case Enum.Gender.Female:
                        return Properties.Resource.Enum_Gender_Female;
                    case Enum.Gender.Other:
                        return Properties.Resource.Enum_Gender_Other;
                    default:
                        return null;
                }

            }
        }


        /// <summary>
        /// Id phòng ban
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        [Date]
        public string? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Vị trí
        /// </summary>
        public string? PositionName { get; set; }


        /// <summary>
        /// Nơi ở
        /// </summary>
        public string? AddressEmployee { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }


        /// <summary>
        /// Số điện thoại bàn
        /// </summary>
        public string? TelephoneNumber { get; set; }


        /// <summary>
        /// email của nhân viên
        /// </summary>
        [Email]
        public string? emailEmployee { get; set; }

        // /// <summary>
        /// Số tài khoản
        /// </summary>
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankAccountName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string? BankAccountBranch { get; set; }

        /// <summary>
        /// Là nhà cung cấp
        /// </summary>
        public int? isCustomer { get; set; }

        /// <summary>
        /// Là nhân viên
        /// </summary>
        public int? isEmployee { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? creatdDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? createdBy { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public DateTime? modifiedDate { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public string? modifiedBy { get; set; }


        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [NotMap]
        public string? DepartmentName { get; set; }


    }
}
