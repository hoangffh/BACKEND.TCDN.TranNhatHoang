using MiSa.Web08.Core.Enum;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace MiSa.Web08.Core
{
    public class Product
    {
        /// <summary>
        /// Id hàng hóa
        /// </summary>
        [PrimaryKey]
        public Guid ProductId { get; set; }


        /// <summary>
        /// Mã hàng hóa 
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string ProductCode { get; set; }


        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        [NotEmpty]
        public string ProductName { get; set; }




        /// <summary>
        /// Mã nhóm vật tư , hàng hóa
        /// </summary>
        [NotMap]
        public string? GroupCategoryCode { get; set; }


        /// <summary>
        /// Id đơn vị tính
        /// </summary>
        public Guid? UnitId { get; set; }

                    
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [NotMap]
        public string? UnitName { get; set; }



        public TaxReduction? TaxReduction { get; set; }


        /// <summary>
        /// Giảm thuế
        /// </summary>
                                                        
        [NotMap]
        public string? TaxReductionValue
        {
            get
            {
                switch (TaxReduction)
                {
                    case Enum.TaxReduction.Reduction:
                        return Properties.Resource.Enum_Type_Reduction;
                    case Enum.TaxReduction.NotDetermined:
                        return Properties.Resource.Enum_Type_NotDetermined;
                    case Enum.TaxReduction.NotReduction:
                        return Properties.Resource.Enum_Type_NotReduction;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Thời hạn bảo hành
        /// </summary>
        public string? Insurance { get; set; }

        /// <summary>
        /// Số lượng tối thiểu
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Nguồn gốc
        /// </summary>
        public string? Source { get; set; }


        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Describes { get; set; }

        /// <summary>
        /// Số lượng tồn
        /// </summary>
        public double? QuantityStock { get; set; }


        /// <summary>
        ///Giá tri tồn
        /// </summary>
        public double? ExistentialValue { get; set; }


        /// <summary>
        /// Diễn giải khi mua
        /// </summary>
        public string? ExplainBuy { get; set; }

        // <summary>
        /// Diễn giải khi bán
        /// </summary>
        public string? ExplainSell { get; set; }

        /// <summary>
        /// Tài khoản giam giá
        /// </summary>
        public string? ReduceAccount { get; set; }

        /// <summary>
        /// Tài khoản kho
        /// </summary>
        public string? WarehouseAccount { get; set; }

        /// <summary>
        /// Tài khoản trả lại
        /// </summary>
        public string? ReturnAccount { get; set; }

        /// <summary>
        ///Tài khoản doanh thu
        /// </summary>
        public string? RevenueAccount { get; set; }

        /// <summary>
        ///Tài khoản chi phí
        /// </summary>
        public string? ExpenseAccount { get; set; }

        /// <summary>
        ///Tài khoản chiết khấu
        /// </summary>
        public string? DiscountAccount { get; set; }


        /// <summary>
        ///Tỉ lệ chiết khấu khi mua
        /// </summary>
        public double? DiscountRate { get; set; }

        /// <summary>
        ///Đơn giá mua cố định
        /// </summary>
        public double? FixedPrice { get; set; }

        /// <summary>
        ///Đơn giá mua gần nhất
        /// </summary>
        public double? NearestPrice { get; set; }

        /// <summary>
        ///Đơn giá bán
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        ///Thué vát
        /// </summary>
        public string? VatTax { get; set; }


        /// <summary>
        //Thuế nhập khẩu
        /// </summary>
        public double? ImportTax { get; set; }

        /// <summary>
        //Thuế xuất khẩu
        /// </summary>
        public double? ExportTax { get; set; }

        /// <summary>
        //Nhóm hàng hóa chịu thuế đặc biệt
        /// </summary>
        public string? SupplyExciseTax { get; set; }


        /// <summary>
        //Id kho
        /// </summary>
        public Guid? WarehouseId { get; set; }

        /// <summary>
        //Mã kho
        /// </summary>
        [NotMap]
        public string? WarehouseCode { get; set; }



        /// <summary>
        /// Ngày tạo
        /// </summary>
        [autoDate]
        public DateTime? createdDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? createdBy { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        [autoDate]
        public DateTime? modifiedOfDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public string? modifiedOfBy { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        [isUsing]
        public Status Status { get; set; } 

        [NotMap]
        public string? UsingStatus
        {
            get
            {
                switch (Status)
                {
                    case Enum.Status.Using:
                        return Properties.Resource.Enum_Type_Using;
                    case Enum.Status.NotUsing:
                        return Properties.Resource.Enum_Type_NotUsing;
                    default:
                        return null;
                }

            }
        }

        public TypeProduct? TypeProduct { get; set; }


        /// <summary>
        //Tính chất hàng hóa
        /// </summary>
        [NotMap]
        public string? TypeProductValue
        {
            get
            {
                switch (TypeProduct)
                {
                    case Enum.TypeProduct.Goods:
                        return Properties.Resource.Enum_Type_Goods;
                    case Enum.TypeProduct.FinishedProduct:
                         return Properties.Resource.Enum_Type_FinishedProduct;
                    case Enum.TypeProduct.RawMaterial:
                        return Properties.Resource.Enum_Type_RawMaterial;
                    case Enum.TypeProduct.Tool:
                        return Properties.Resource.Enum_Type_Tool;
                    case Enum.TypeProduct.Service:
                        return Properties.Resource.Enum_Type_Service;
                    default:
                        return null;
                }
            }
        }


        [NotMap]
        public List<Guid>? GroupCategoryListId { get; set; }
    }
}
