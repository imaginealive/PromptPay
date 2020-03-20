using Saladpuk.EMVCo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saladpuk.EMVCo.Tests.Models
{
    public class EMVCoStandard
    {
        public QrIdentifier id { get; }
        public LengthRole role { get; }
        public int length { get; }
        public QrIdentifierPresence presence { get; }
        public QrIdentifierFormat format { get; }
        public EMVCoStandard(QrIdentifier id, LengthRole role, int length, QrIdentifierPresence presence, QrIdentifierFormat format)
        {
            this.id = id;
            this.role = role;
            this.length = length;
            this.presence = presence;
            this.format = format;
        }
    }
    public enum LengthRole
    {
        Fixed,
        Limit
    }
    public enum QrIdentifierPresence
    {
        Mandatory,
        Conditional,
        Optional
    }
    public enum QrIdentifierFormat
    {
        Number,
        AlphanumericSpecial,
        String
    }
}
