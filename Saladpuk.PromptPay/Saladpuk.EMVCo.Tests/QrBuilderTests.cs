using FluentAssertions;
using Saladpuk.EMVCo.Contracts;
using Saladpuk.EMVCo.Tests.Models;
using Saladpuk.PromptPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Saladpuk.EMVCo.Tests
{
    public class QrBuilderTests
    {
        private PromptPayQrBuilder sut = new PromptPayQrBuilder();
        private PromptPayQrReader reader = new PromptPayQrReader();

        [Theory]
        [MemberData(nameof(QRInfoTestData))]
        public void BuildQrOnEMVCoStandard(params QrInfo[] input)
        {
            input.ToList().ForEach(it => sut.Add(it.id, it.value));
            var autual = reader.Read(sut.ToString());
            var mandatoryList = EMVCoStandardsData.EMVCoStandards.Where(it => it.presence == QrIdentifierPresence.Mandatory).Select(it => new QrIdentifierCheckItem { id = it.id, IsCheck = false });
            autual.Segments.ToList().ForEach(Segment =>
            {
                var EMV = EMVCoStandardsData.EMVCoStandards.FirstOrDefault(it => it.id == Segment.IdByConvention);
                var convertible = int.TryParse(Segment.Length, out var length);
                switch (EMV.role)
                {
                    case LengthRole.Fixed: length.Should().Be(EMV.length); break;
                    case LengthRole.Limit: length.Should().BeLessOrEqualTo(EMV.length); break;
                    default: break;
                }
                switch (EMV.format)
                {
                    case QrIdentifierFormat.Number:
                        Segment.Value.All(char.IsDigit).Should().BeTrue();
                        break;
                    case QrIdentifierFormat.String:
                        Segment.Value.All(char.IsLetterOrDigit).Should().BeTrue();
                        break;
                    default: break;
                }
            });
            mandatoryList = mandatoryList.Select(it =>
            {
                if (autual.Segments.Any(Segment => Segment.IdByConvention == it.id))
                    it.IsCheck = true;
                return it;
            });
            mandatoryList.All(it => it.IsCheck).Should().BeTrue();
        }
        public static IEnumerable<object[]> QRInfoTestData => new List<QrInfo[]>
        {
            new QrInfo[]{
                new QrInfo{ id = QrIdentifier.PayloadFormatIndicator, value = "00"},
                new QrInfo{ id = QrIdentifier.MerchantAccountInformation, value = "Merchant Account"},
                new QrInfo{ id = QrIdentifier.MerchantCategoryCode, value = "1780"},
                new QrInfo{ id = QrIdentifier.TransactionCurrency, value = "764"},
                new QrInfo{ id = QrIdentifier.CountryCode, value = "TH"},
                new QrInfo{ id = QrIdentifier.MerchantName, value = "Merchant Name"},
                new QrInfo{ id = QrIdentifier.MerchantCity, value = "Merchant City"},
                new QrInfo{ id = QrIdentifier.CRC, value = "F4E8"},
            }
        };
    }
    public class QrInfo {
        public QrIdentifier id { get; set; }
        public string value { get; set; }
    }
}
