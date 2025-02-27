﻿using Microsoft.AspNetCore.Mvc;
using PaymentService.Share;

namespace PaymentService.ServicePayment.MoMo
{
    [BindProperties]
    public class MomoOneTimePaymentResultRequest
    {
        
        public string? partnerCode { get; set; } = string.Empty;
        public string? orderId { get; set; } = string.Empty;
        public string? requestId { get; set; } = string.Empty;
        public long amount { get; set; }
        public string? orderInfo { get; set; } = string.Empty;
        public string? orderType { get; set; } = string.Empty;
        public string? transId { get; set; } = string.Empty;
        public string? message { get; set; } = string.Empty;
        public int resultCode { get; set; }
        public string? payType { get; set; } = string.Empty;
        public long responseTime { get; set; }
        public string? extraData { get; set; } = string.Empty;
        public string? signature { get; set; } = string.Empty;
        public int? UserCreateCourseId { get; set; }
        public int? CourseId { get; set; }

        public bool IsValidSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                   "&amount=" + this.amount +
                   "&extraData=" + this.extraData +
                   "&message=" + this.message +
                   "&orderId=" + this.orderId +
                   "&orderInfo=" + this.orderInfo +
                   "&orderType=" + this.orderType +
                   "&partnerCode=" + this.partnerCode +
                   "&payType=" + this.payType +
                   "&requestId=" + this.requestId +
                   "&responseTime=" + this.responseTime +
                   "&resultCode=" + this.resultCode +
                   "&transId=" + this.transId;
            var checkSignature = HashHelper.HmacSHA256(rawHash, secretKey);
            return this.signature.Equals(checkSignature);
        }
    }
}
