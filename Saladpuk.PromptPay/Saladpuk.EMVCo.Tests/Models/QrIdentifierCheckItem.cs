using Saladpuk.EMVCo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saladpuk.EMVCo.Tests.Models
{
    public class QrIdentifierCheckItem
    {
        public QrIdentifier id { get; set; }
        public bool IsCheck { get; set; }
    }
}
