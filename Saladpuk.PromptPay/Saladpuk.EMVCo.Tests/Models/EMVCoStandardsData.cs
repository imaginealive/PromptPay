using Saladpuk.EMVCo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saladpuk.EMVCo.Tests.Models
{
    static class EMVCoStandardsData
    {
        public static IEnumerable<EMVCoStandard> EMVCoStandards = new List<EMVCoStandard>
        {
            new EMVCoStandard(QrIdentifier.PayloadFormatIndicator, LengthRole.Fixed, 2, QrIdentifierPresence.Mandatory, QrIdentifierFormat.Number),
            new EMVCoStandard(QrIdentifier.PointOfInitiationMethod, LengthRole.Fixed, 2, QrIdentifierPresence.Optional, QrIdentifierFormat.Number),
            new EMVCoStandard(QrIdentifier.MerchantAccountInformation, LengthRole.Limit, 99, QrIdentifierPresence.Mandatory, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.MerchantCategoryCode, LengthRole.Fixed, 4, QrIdentifierPresence.Mandatory, QrIdentifierFormat.Number),
            new EMVCoStandard(QrIdentifier.TransactionCurrency, LengthRole.Fixed, 3, QrIdentifierPresence.Mandatory, QrIdentifierFormat.Number),
            new EMVCoStandard(QrIdentifier.TransactionAmount, LengthRole.Limit, 13, QrIdentifierPresence.Conditional, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.TipOrConvenienceIndicator, LengthRole.Fixed, 2, QrIdentifierPresence.Optional, QrIdentifierFormat.Number),
            new EMVCoStandard(QrIdentifier.ValueOfConvenienceFeeFixed, LengthRole.Limit, 13, QrIdentifierPresence.Conditional, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.ValueOfConvenienceFeePercentage, LengthRole.Limit, 5, QrIdentifierPresence.Conditional, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.CountryCode, LengthRole.Fixed, 2, QrIdentifierPresence.Mandatory, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.MerchantName, LengthRole.Limit, 25, QrIdentifierPresence.Mandatory, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.MerchantCity, LengthRole.Limit, 15, QrIdentifierPresence.Mandatory, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.PostalCode, LengthRole.Limit, 10, QrIdentifierPresence.Optional, QrIdentifierFormat.AlphanumericSpecial),
            new EMVCoStandard(QrIdentifier.AdditionalData, LengthRole.Limit, 99, QrIdentifierPresence.Optional, QrIdentifierFormat.String),
            new EMVCoStandard(QrIdentifier.MerchantInformationLanguageTemplate, LengthRole.Limit, 99, QrIdentifierPresence.Optional, QrIdentifierFormat.String),
            new EMVCoStandard(QrIdentifier.RFU, LengthRole.Limit, 99, QrIdentifierPresence.Optional, QrIdentifierFormat.String),
            new EMVCoStandard(QrIdentifier.UnreservedTemplates, LengthRole.Limit, 99, QrIdentifierPresence.Optional, QrIdentifierFormat.String),
            new EMVCoStandard(QrIdentifier.CRC, LengthRole.Fixed, 4, QrIdentifierPresence.Mandatory, QrIdentifierFormat.AlphanumericSpecial),
        };
    }
}