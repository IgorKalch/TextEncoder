var encoder = new TextEncoder(escapingCharacter: ',', encodingMap: new Dictionary<char, char> {
     { ' ', '1' }
});

var encoded = encoder.Encode("a b");
Console.WriteLine(encoded);

var decoded = encoder.Decode("a,1b");
Console.WriteLine(decoded);

Console.ReadKey();