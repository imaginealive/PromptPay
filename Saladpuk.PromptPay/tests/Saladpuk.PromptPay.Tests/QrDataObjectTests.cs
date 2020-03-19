using FluentAssertions;
using Saladpuk.EMVCo.Models;
using Saladpuk.PromptPay.Models;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrDataObjectTests
    {
        [Theory]
        [InlineData("AABBC", "AA", "BB", "C")]
        [InlineData("5802TH", "58", "02", "TH")]
        public void CorrectQrDataObject(string rawValue, string expectedId, string expectedLength, string expectedValue)
        {
            var sut = new QrDataObject(rawValue);
            sut.RawValue.Should().BeEquivalentTo(rawValue);
            sut.Id.Should().BeEquivalentTo(expectedId);
            sut.Length.Should().BeEquivalentTo(expectedLength);
            sut.Value.Should().BeEquivalentTo(expectedValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0000")]
        public void IncorrentQrDataObject(string rawValue)
        {
            var result = false;
            try
            {
                var sut = new QrDataObject(rawValue);
                result = true;
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                result.Should().BeFalse();
            }

        }
        // TODO: Test cases
        // Invalid Id.
        // Invalid length.
        // Invalid value's length & format.
    }
}
