﻿using FluentAssertions;
using Saladpuk.EMVCo.Contracts;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrBuilderTests
    {
        private PromptPayQrBuilder sut = new PromptPayQrBuilder();

        [Fact]
        public void DefaultQrBuilderMustBeEmpty()
            => sut.ToString().Should().BeEquivalentTo(string.Empty);

        [Theory]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "01", "000201")]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "02", "000202")]
        public void SimpleGeneration(QrIdentifier id, string value, string expected)
            => sut.Add(id, value).ToString().Should().BeEquivalentTo(expected);

        [Theory]
        [InlineData(0, "54040.00")]
        [InlineData(50, "540550.00")]
        [InlineData(50.00, "540550.00")]
        [InlineData(101.001, "5406101.00")]
        [InlineData(102.004, "5406102.00")]
        [InlineData(201.005, "5406201.01")]
        [InlineData(202.0054, "5406202.01")]
        [InlineData(203.0055, "5406203.01")]
        [InlineData(93, "540593.00")]
        [InlineData(008, "54048.00")]
        [InlineData(050.34633, "540550.35")]
        [InlineData(.654246, "54040.65")]
        // TODO: Test cases
        //ไม่มีตัวเลขหลังจุด ทำให้error และยังไม่ได้ข้อสรุปของคำตอบ
        //[InlineData(0050., "540550.00")]
        //[InlineData(-0020., "5540520.00")]


        public void AddTransactionAmount(double amount, string expected)
            => sut.SetTransactionAmount(amount).ToString().Should().BeEquivalentTo(expected);

        [Theory]
        [InlineData(-50, "540550.00")]
        [InlineData(-0, "54040.00")]
        [InlineData(-004, "54044.00")]
        [InlineData(-.007657, "54040.01")]
        [InlineData(-50.00, "540550.00")]
        [InlineData(-101.001, "5406101.00")]
        [InlineData(-102.004, "5406102.00")]
        [InlineData(-201.005, "5406201.01")]
        [InlineData(-202.0054, "5406202.01")]
        [InlineData(-203.0055, "5406203.01")]
        [InlineData(-93, "540593.00")]
        [InlineData(-008, "54048.00")]
        [InlineData(-050.34633, "540550.35")]
        [InlineData(-.654246, "54040.65")]
        // TODO: Test cases
        //ไม่มีตัวเลขหลังจุด ทำให้error และยังไม่ได้ข้อสรุปของคำตอบ
        //[InlineData(-0020., "540520.00")]
        // TODO: Test cases
        //ไม่สามารถใส่จุดได้มากกว่า 2 และยังไม่ได้ข้อสรุปของคำตอบ
        //[InlineData(-.., "54040.00")]
        //[InlineData(-0.., "54040.00")]
        // TODO: Test cases
        //ไม่สามารถใส่ตัวอักษร อักขระได้ และยังไม่ได้ข้อสรุปของคำตอบ
        //[InlineData(AA.BB, "54040.00")]
        //[InlineData(-AA.BB, "54040.00")]
        //[InlineData(AA, "54040.00")]
        //[InlineData(-AA, "54040.00")]

        public void AddTransactionAmountWithNegativeValue(double amount, string expected)
            => sut.SetTransactionAmount(amount).ToString().Should().BeEquivalentTo(expected);

        // TODO: Test cases
        // The Payload Format Indicator (ID "00") shall be the first data object in the QR Code.
        // The CRC (ID "63") shall be the last data object in the QR Code. All other data objects under the root may be placed at any position.
        // The length of the payload should not exceed 512 alphanumeric characters.

        // PayloadFormatIndicator, MerchantAccountInformation, MerchantCategoryCode, TransactionCurrency, CountryCode and MerchantName shall be present under the root of the QR Code.
        // If MerchantInformationLanguage Template (ID "64") is present, then the data objects that are labelled[M] in Table 3.8 shall be present in the template.
        // If the Additional Data Field Template(ID "62") is present, then the data objects that are labelled[O] in Table 3.7 may be present in this template.
        // Transaction Amount shall only include (numeric) digits "0" to "9" and may contain a single "." character as the decimal mark even if there are no decimals.
        // The Transaction Currency shall conform to [ISO 4217] and shall contain the 3-digit numeric representation of the currency.
    }
}
