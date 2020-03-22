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
using System;

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

        [Fact]
        public void ReadEmpty() => sut.Read(string.Empty).Segments.Count().Should().Be(0);

        [Fact]
        public void ReadNull() => sut.Read(null).Segments.Count().Should().Be(0);

        [Theory]
        [InlineData("000200000201", "000200", "000201")]
        [InlineData("5802EN5802TH", "5802EN", "5802TH")]
        [InlineData("0002015802EN53037645802TH", "000201", "5802EN", "5303764", "5802TH")]
        [InlineData("540599.995405AA.AA", "540599.99", "5405AA.AA")]
        [InlineData("0002005802DM5406123.450002015802TH540545.50", "000200", "5802DM", "5406123.45", "000201", "5802TH", "540545.50")]
        [InlineData("0216421817000006300902164218170000744533", "02164218170000063009", "02164218170000744533")]
        [InlineData("5204344352049866", "52043443", "52049866")]
        [InlineData("5920EK CHAI DISTRIBUTION5907EK CHAI", "5920EK CHAI DISTRIBUTION", "5907EK CHAI")]
        [InlineData("6008KHONKEAN6007BANGKOK", "6008KHONKEAN", "6007BANGKOK")]
        [InlineData("6304G7646304HJ76", "6304G764", "6304HJ76")]
        [InlineData("623201061225260506122522607089140007623201061225260506122522605557763342", "623201061225260506122522607089140007", "623201061225260506122522605557763342")]
        [InlineData("64200002ZH0104最佳运输0202北京64200002TH0104最佳运输0202北京", "64200002ZH0104最佳运输0202北京", "64200002TH0104最佳运输0202北京")]
        [InlineData("550201550202", "550201", "550202")]
        [InlineData("560576446560534233", "560576446", "560534233")]
        [InlineData("57054433356043221", "570544333", "56043221")]
        [InlineData("6104776561049980", "61047765", "61049980")]
        [InlineData("6510756453342565107564523316", "65107564533425", "65107564523316")]
        [InlineData("8005666348006887723", "800566634", "8006887723")]
        public void TwoMoreSameIDQRMustBeReadable(string qrcode, params string[] expected)
        {
            var expectedSegment = expected.Select(it => new QrDataObject(it));
            var actual = sut.Read(qrcode);
            actual.Segments.Should().BeEquivalentTo(expectedSegment);
            var expectedGroup = expectedSegment.GroupBy(it => it.IdByConvention);
            expectedGroup.ToList().ForEach(it => {
                switch (it.Key)
                {
                    case QrIdentifier.PayloadFormatIndicator:
                        actual.PayloadFormatIndicator.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.PointOfInitiationMethod:
                        actual.PointOfInitiationMethod.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.MerchantAccountInformation:
                        actual.MerchantAccountInformation.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.MerchantCategoryCode:
                        actual.MerchantCategoryCode.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.TransactionCurrency:
                        actual.TransactionCurrency.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.TransactionAmount:
                        actual.TransactionAmount.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.TipOrConvenienceIndicator:
                        actual.TipOrConvenienceIndicator.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.ValueOfConvenienceFeeFixed:
                        actual.ValueOfConvenienceFeeFixed.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.ValueOfConvenienceFeePercentage:
                        actual.ValueOfConvenienceFeePercentage.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.CountryCode:
                        actual.CountryCode.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.MerchantName:
                        actual.MerchantName.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.MerchantCity:
                        actual.MerchantCity.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.PostalCode:
                        actual.PostalCode.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.AdditionalData:
                        actual.AdditionalData.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.CRC:
                        actual.CRC.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.MerchantInformationLanguageTemplate:
                        actual.MerchantInformationLanguageTemplate.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.RFU:
                        actual.RFU.Should().BeEquivalentTo(it.Last().Value);
                        break;
                    case QrIdentifier.UnreservedTemplates:
                        break;
                }
            });
        }

        [Theory]
        [InlineData("00020")]
        [InlineData("000A01")]
        [InlineData("000000")]
        public void ReadIncorrectFormatQrCode(string qrCode)
        {
            Action testProcess = () => sut.Read(qrCode);
            testProcess.Should().Throw<Exception>();
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
