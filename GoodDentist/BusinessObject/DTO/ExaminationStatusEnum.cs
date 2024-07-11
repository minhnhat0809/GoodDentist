using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public enum ExaminationStatusEnum
    {
            [Description("Đang đến")]
            DangDen = 0,

            [Description("Đã đến")]
            DaDen = 1,

            [Description("Đã huỷ")]
            DaHuy = 2,

            [Description("Đã xoá")]
            HoanThanh = 3,
    }
}
