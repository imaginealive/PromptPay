using FluentAssertions;
using Saladpuk.EMVCo.Models;
using Saladpuk.PromptPay.Facades;
using Saladpuk.PromptPay.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Linq;
using FluentAssertions;
using Saladpuk.EMVCo.Models;
using Saladpuk.EMVCo.Contracts;

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
            actual.CountryCode.Should().BeEquivalentTo(expectedSegment.Last(it => it.IdByConvention == QrIdentifier.CountryCode).Value);
        } 
        
        [Theory]
        [InlineData("00020")]
        [InlineData("000A01")]
        [InlineData("000000")]
        public void ReadIncorrectFormatQrCode(string qrCode)
        {
            var convertible = false;
            try
            {
                var autual = sut.Read(qrCode);
                convertible = true;
            }
            catch (System.Exception)
            {
            }
            finally
            {
                convertible.Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("5303764630401F8", "5303764", "630401F8")]
        [InlineData("00020101021229370016A000000677010111011300669141854015303764540550.005802TH630401F8", "000201", "010212", "29370016A00000067701011101130066914185401", "5303764", "540550.00", "5802TH", "630401F8")]
        public void ReadCorrentFormatQrCode(string QrCode, params string[] expected)
        {
            var autual = sut.Read(QrCode);
            var epectedSegments = expected.Select(it => new QrDataObject(it));
            autual.Segments.Should().BeEquivalentTo(epectedSegments);
        }

        [Theory]
        [InlineData("000210123", "000210")]
        [InlineData("0002115406500.001", "000211", "5406500.00")]
        public void ReadCorrectFormatWithSomeUnknownstringQrCode(string QrCode, params string[] expected)
        {
            var autual = sut.Read(QrCode);
            var epectedSegments = expected.Select(it => new QrDataObject(it));
            autual.Segments.Should().BeEquivalentTo(epectedSegments);
        }
        // TODO: Test cases
        // The length of the payload should not exceed 512 alphanumeric characters.
    }
}
