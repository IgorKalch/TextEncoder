using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;

namespace TextEncoderTest;

[TestClass]
public class TextEncoderTests
{
    [TestMethod]
    [DataRow("a b", "a,1b")]
    [DataRow("a b c", "a,1b,1c")]
    [DataRow("", "")]
    [DataRow("a,b,c d", "a,b,c,1d")]
    [DataRow("a,b,c", "a,b,c")]
    [DataRow("a,b,c", "a,b,c")]
    public void Encode_Success(string initial, string expected)
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');

        var result = encoder.Encode(initial);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Encode_ErrorNull()
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');
        encoder.Encode(null);
    }

    [TestMethod]
    [DataRow("a,1b", "a b")]
    [DataRow("a,1b,1c", "a b c")]
    [DataRow("", "")]
    [DataRow("a,b,c", "a,b,c")]
    [DataRow("a,b,c,1d", "a,b,c d")]
    public void Decode_Success(string initial, string expected)
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');

        var result = encoder.Decode(initial);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Decode_ErrorNull()
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');
        encoder.Decode(null);
    }

    [TestMethod]
    [DataRow("a b", "a b")]
    [DataRow(" ", " ")]
    [DataRow("", "")]
    public void Constructor_EncodingMapNull_Encode_ReturnsOriginalText(string initial, string expected)
    {
        var encoder = new TextEncoder(encodingMap: null, escapingCharacter: ',');
        var result = encoder.Encode(initial);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("a b", "a b")]
    [DataRow(" ", " ")]
    [DataRow("", "")]
    public void Constructor_EncodingMapNull_Decode_ReturnsOriginalText(string initial, string expected)
    {
        var encoder = new TextEncoder(encodingMap: null, escapingCharacter: ',');
        var result = encoder.Decode(initial);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PerformanceTest_Encode()
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');
        const int iterations = 10000000;

        var input = new StringBuilder();
        input.Append('a', iterations);
        input.Append(' ', iterations);
        input.Append('b', iterations);

        var stopwatch = Stopwatch.StartNew();
        encoder.Encode(input.ToString());
        stopwatch.Stop();

        Console.WriteLine($"Encoding performance test completed in {stopwatch.ElapsedMilliseconds} ms.");
    }

    [TestMethod]
    public void PerformanceTest_Decode()
    {
        var encoder = new TextEncoder(encodingMap: new Dictionary<char, char> { { ' ', '1' } }, escapingCharacter: ',');
        const int iterations = 10000000;

        var input = new StringBuilder();
        input.Append('a', iterations);
        input.Append(' ', iterations);
        input.Append('b', iterations);

        var encodedText = encoder.Encode(input.ToString());

        var stopwatch = Stopwatch.StartNew();
        encoder.Decode(encodedText);
        stopwatch.Stop();

        Console.WriteLine($"Decoding performance test completed in {stopwatch.ElapsedMilliseconds} ms.");
    }
}