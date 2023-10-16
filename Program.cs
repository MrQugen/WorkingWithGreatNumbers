namespace WorkingWithGreatNumbers;

public static class Program
{
    public static void Main(string[] args)
    {
        // BigInteger bigInt1 = new BigInteger("50");
        // BigInteger bigInt2 = new BigInteger("25");
        // BigInteger bigInt3 = new BigInteger("1000000000000");
        //
        // Console.WriteLine("BigInteger 1: " + bigInt1);
        // Console.WriteLine("BigInteger 2: " + bigInt2);
        // Console.WriteLine("BigInteger 3: " + bigInt3);
        //
        // // Testing Addition
        // bigInt1.Add(bigInt2);
        // Console.WriteLine("After Add: " + bigInt1);
        //
        // // Testing Subtraction
        // bigInt1.Sub(bigInt2);
        // Console.WriteLine("After Sub: " + bigInt1);
        //
        // // Testing Multiplication
        // bigInt1.Mul(bigInt2);
        // Console.WriteLine("After Mul: " + bigInt1);
        //
        // // Testing Division
        // BigInteger quotient, remainder;
        // bigInt1.Div(bigInt3, out quotient, out remainder);
        // Console.WriteLine("Quotient: " + quotient.Hex);
        // Console.WriteLine("Remainder: " + remainder.Hex);
        //
        // // Testing Modulus
        // bigInt2.Mod(bigInt3);
        // Console.WriteLine("After Mod: " + bigInt2.Hex);

        //
        // Примеры из файла
        //
        
        // XOR
        var num1 = new BigInteger("0");
        var num2 = new BigInteger("0");
        
        num1.Hex = "51bf608414ad5726a3c1bec098f77b1b54ffb2787f8d528a74c1d7fde6470ea4";
        num2.Hex = "403db8ad88a3932a0b7e8189aed9eeffb8121dfac05c3512fdb396dd73f6331c";
        num1.Xor(num2);
        Console.WriteLine(num1.Hex);
        
        // ADD - не сходится
        num1.Hex = "36f028580bb02cc8272a9a020f4200e346e276ae664e45ee80745574e2f5ab80";
        num2.Hex = "70983d692f648185febe6d6fa607630ae68649f7e6fc45b94680096c06e4fadb";
        num1.Add(num2);
        Console.WriteLine(num1.Hex);
        
        // SUB
        num1.Hex = "33ced2c76b26cae94e162c4c0d2c0ff7c13094b0185a3c122e732d5ba77efebc";
        num2.Hex = "22e962951cb6cd2ce279ab0e2095825c141d48ef3ca9dabf253e38760b57fe03";
        num1.Sub(num2);
        Console.WriteLine(num1.Hex);
        
        // MUL - не сходится
        num1.Hex = "7d7deab2affa38154326e96d350deee1";
        num2.Hex = "97f92a75b3faf8939e8e98b96476fd22";
        num1.Mul(num2);
        Console.WriteLine(num1.Hex);
    }
}