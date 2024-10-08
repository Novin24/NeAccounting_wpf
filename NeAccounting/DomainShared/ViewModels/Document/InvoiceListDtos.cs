﻿using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class InvoiceListDtos
    {
        public int Row { get; set; }
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? ShamsiDate { get; set; }
        public string? Description { get; set; }
        public long Bes { get; set; }
        public long Bed { get; set; }
        public long LeftOver { get; set; }
        public string? Serial { get; set; }
        public string? Status { get; set; }
        public bool IsDeletable { get; set; } = false;
        public bool IsEditable { get; set; } = false;
        public bool IsPrintable { get; set; } = false;
        /// <summary>
        /// ایا  دکمه اجناس بازگشتی داشته باشد یا خیر
        /// </summary>
        public bool HaveReturned { get; set; } = false;
        /// <summary>
        /// شناسه فاکتور والد درصورت لزوم
        /// </summary>
        public Guid? ParentId { get; set; }
        public DocumntType Type { get; set; }
    }
}
