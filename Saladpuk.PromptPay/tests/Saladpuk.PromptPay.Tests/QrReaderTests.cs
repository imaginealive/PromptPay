﻿using Saladpuk.PromptPay.Facades;
using Saladpuk.PromptPay.Models;
using Xunit;
using System.Linq;
using FluentAssertions;
using Saladpuk.EMVCo.Models;

namespace Saladpuk.PromptPay.Tests
{
    public class QrReaderTests
    {
        private PromptPayQrReader sut = new PromptPayQrReader();

        [Fact]
        public void DefaultStaticCreditTransferQRMustBeReadable()
        {
            var qrCode = PPay.StaticQR.CreateCreditTransferQrCode();
            var actual = sut.Read(qrCode);
            var expected = new PromptPayQrInfo()
                .InitializeDefault()
                .SetPlainCreditTransfer();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultDynamicCreditTransferQRMustBeReadable()
        {
            var qrCode = PPay.DynamicQR.CreateCreditTransferQrCode();
            var actual = sut.Read(qrCode);
            var expected = new PromptPayQrInfo()
                .InitializeDefault(staticQr: false)
                .SetPlainCreditTransfer();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultStaticBillPaymentQRMustBeReadable()
        {
            var qrCode = PPay.StaticQR.CreateBillPaymentQrCode();
            var actual = sut.Read(qrCode);
            var expected = new PromptPayQrInfo()
                .InitializeDefault()
                .SetPlainBillPayment();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultDynamicBillPaymentQRMustBeReadable()
        {
            var qrCode = PPay.DynamicQR.CreateBillPaymentQrCode();
            var actual = sut.Read(qrCode);
            var expected = new PromptPayQrInfo()
                .InitializeDefault(staticQr: false)
                .SetPlainBillPayment();
            actual.ValidateWith(expected);
        }


        [Theory]
        [InlineData("5802EN5802TH", new string[] { "5802EN", "5802TH" })]
        public void TwoMoreSameIDQRMustBeReadable(string qrcode, string[] expected)
        {
            var expectedSegment = expected.Select(it => new QrDataObject(it));
            var actual = sut.Read(qrcode);
            actual.Segments.Should().BeEquivalentTo(expectedSegment);
            actual.CountryCode.Should().BeEquivalentTo(expectedSegment.Last(it => it.Id == "58").Value);
 
        }
        // TODO: Test cases
        // The length of the payload should not exceed 512 alphanumeric characters.
    }
}
