using System.Collections.Frozen;
using System.Text;

public class TextEncoder
{
    private readonly char _escapingCharacter;
    private readonly FrozenDictionary<char, char> _encodingMap;
    private readonly FrozenDictionary<char, char> _decodingMap;

    /// <summary>
    /// Initializes a new instance of the TextEncoder class.
    /// </summary>
    /// <param name="encodingMap">A dictionary specifying character replacements for encoding.</param>
    /// <param name="escapingCharacter">The character used for escaping.</param>
    public TextEncoder(Dictionary<char, char> encodingMap, char escapingCharacter = ',')
    {
        _escapingCharacter = escapingCharacter;
        _encodingMap = (encodingMap ?? new Dictionary<char, char>()).ToFrozenDictionary();
        _decodingMap = GenerateDecodingMap(encodingMap);
    }

    /// <summary>
    /// Encodes a given text based on the configured encoding map.
    /// </summary>
    /// <param name="text">The input text to encode.</param>
    /// <returns>The encoded text.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input text is null.</exception>
    public string Encode(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var encodedText = new StringBuilder();

        foreach (var c in text)
        {
            if (_encodingMap.TryGetValue(c, out var replacement))
            {
                encodedText.Append(_escapingCharacter);
                encodedText.Append(replacement);
            }
            else
            {
                encodedText.Append(c);
            }
        }

        return encodedText.ToString();
    }

    /// <summary>
    /// Decodes a given encoded text based on the configured decoding map.
    /// </summary>
    /// <param name="encodedText">The encoded text to decode.</param>
    /// <returns>The decoded text.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input encoded text is null.</exception>
    public string Decode(string encodedText)
    {
        ArgumentNullException.ThrowIfNull(encodedText);

        var decodedText = new StringBuilder();
        var isEscaped = false;

        foreach (var c in encodedText)
        {
            if (isEscaped)
            {
                if (_decodingMap.TryGetValue(c, out var original))
                {
                    decodedText.Append(original);
                }
                else
                {
                    decodedText.Append(_escapingCharacter);
                    decodedText.Append(c);
                }

                isEscaped = false;
            }
            else if (c == _escapingCharacter)
            {
                isEscaped = true;
            }
            else
            {
                decodedText.Append(c);
            }
        }

        return decodedText.ToString();
    }

    private FrozenDictionary<char, char> GenerateDecodingMap(Dictionary<char, char> encodingMap)
    {
        var decodingMap = new Dictionary<char, char>();

        if (encodingMap != null)
        {
            foreach (var entry in encodingMap)
            {
                decodingMap[entry.Value] = entry.Key;
            }
        }           

        return decodingMap.ToFrozenDictionary();
    }
}
