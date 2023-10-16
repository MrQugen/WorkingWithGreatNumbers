namespace WorkingWithGreatNumbers;

class BigInteger
{
        private ulong[] data;

        public static BigInteger Zero => new BigInteger("0");
        public static BigInteger One => new BigInteger("1");

        public BigInteger(string numberStr)
        {
            int blockCount = (numberStr.Length + 15) / 16;
            data = new ulong[blockCount];

            for (int i = 0; i < blockCount; i++)
            {
                int startIndex = Math.Max(0, numberStr.Length - (i + 1) * 16);
                int length = Math.Min(16, numberStr.Length - startIndex);
                string blockStr = numberStr.Substring(startIndex, length);
                data[i] = ulong.Parse(blockStr);
            }
        }

        public byte[] ByteArray
        {
            get
            {
                int size = data.Length * 8;
                byte[] bytes = new byte[size];

                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        bytes[i * 8 + j] = (byte)(data[i] & 0xFF);
                    }
                }

                return bytes;
            }
            set
            {
                if (value.Length % 8 != 0)
                {
                    throw new ArgumentException("Byte array length must be a multiple of 8.");
                }

                int blockCount = value.Length / 8;
                data = new ulong[blockCount];

                for (int i = 0; i < blockCount; i++)
                {
                    byte[] blockBytes = new byte[8];
                    Array.Copy(value, i * 8, blockBytes, 0, 8);
                    data[i] = BitConverter.ToUInt64(blockBytes, 0);
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = data.Length - 1; i >= 0; i--)
            {
                result += data[i];
            }
            return result.TrimStart('0');
        }

        public string Hex
        {
            get
            {
                string result = "";
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    string hexStr = data[i].ToString("x16");
                    result += hexStr.TrimStart('0');
                }
                return result.Length == 0 ? "0" : result;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Hex string cannot be empty.");
                }

                value = value.TrimStart('0');

                int blockCount = (value.Length + 15) / 16;
                data = new ulong[blockCount];

                for (int i = 0; i < blockCount; i++)
                {
                    int startIndex = Math.Max(0, value.Length - (i + 1) * 16);
                    int length = Math.Min(16, value.Length - startIndex);
                    string blockStr = value.Substring(startIndex, length);
                    data[i] = Convert.ToUInt64(blockStr, 16);
                }
            }
        }

        public void Not()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ~data[i];
            }
        }

        public void Xor(BigInteger other)
        {
            if (data.Length != other.data.Length)
            {
                throw new ArgumentException("Both operands must have the same length.");
            }

            for (int i = 0; i < data.Length; i++)
            {
                ulong a = data[i];
                ulong b = other.data[i];
                ulong result = 0;

                for (int bitPosition = 0; bitPosition < 64; bitPosition++)
                {
                    ulong aBit = (a >> bitPosition) & 1;
                    ulong bBit = (b >> bitPosition) & 1;
                    ulong xorResult = (aBit ^ bBit) << bitPosition;
                    result |= xorResult;
                }

                data[i] = result;
            }
        }

        public void Or(BigInteger other)
        {
            if (data.Length != other.data.Length)
            {
                throw new ArgumentException("Both operands must have the same length.");
            }

            for (int i = 0; i < data.Length; i++)
            {
                ulong a = data[i];
                ulong b = other.data[i];
                ulong result = 0;

                for (int bitPosition = 0; bitPosition < 64; bitPosition++)
                {
                    ulong aBit = (a >> bitPosition) & 1;
                    ulong bBit = (b >> bitPosition) & 1;
                    ulong orResult = (aBit | bBit) << bitPosition;
                    result |= orResult;
                }

                data[i] = result;
            }
        }

        public void And(BigInteger other)
        {
            if (data.Length != other.data.Length)
            {
                throw new ArgumentException("Both operands must have the same length.");
            }

            for (int i = 0; i < data.Length; i++)
            {
                ulong a = data[i];
                ulong b = other.data[i];
                ulong result = 0;

                for (int bitPosition = 0; bitPosition < 64; bitPosition++)
                {
                    ulong aBit = (a >> bitPosition) & 1;
                    ulong bBit = (b >> bitPosition) & 1;
                    ulong andResult = (aBit & bBit) << bitPosition;
                    result |= andResult;
                }

                data[i] = result;
            }
        }

        public void ShiftRight(int shiftAmount)
        {
            if (shiftAmount < 0)
            {
                throw new ArgumentException("Shift amount must be non-negative.");
            }

            int blockShift = shiftAmount / 64;
            int bitShift = shiftAmount % 64;

            if (blockShift >= data.Length)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
            }
            else
            {
                for (int i = data.Length - 1; i >= blockShift; i--)
                {
                    if (i - blockShift < data.Length)
                    {
                        data[i] = i - blockShift - 1 >= 0 ? (data[i - blockShift] >> bitShift) | (data[i - blockShift - 1] << (64 - bitShift)) : (data[i - blockShift] >> bitShift);
                    }
                    else
                    {
                        data[i] = 0;
                    }
                }

                for (int i = 0; i < blockShift; i++)
                {
                    data[i] = 0;
                }
            }
        }

        public void ShiftLeft(int shiftAmount)
        {
            if (shiftAmount < 0)
            {
                throw new ArgumentException("Shift amount must be non-negative.");
            }

            int blockShift = shiftAmount / 64;
            int bitShift = shiftAmount % 64;

            for (int i = 0; i < data.Length; i++)
            {
                if (i + blockShift < data.Length)
                {
                    data[i] = i + blockShift + 1 < data.Length ? (data[i + blockShift] << bitShift) | (data[i + blockShift + 1] >> (64 - bitShift)) : (data[i + blockShift] << bitShift);
                }
                else
                {
                    data[i] = 0;
                }
            }
        }

        public void Add(BigInteger other)
        {
            int maxLength = Math.Max(data.Length, other.data.Length) + 1;
            ulong[] result = new ulong[maxLength];
            ulong carry = 0;

            for (int i = 0; i < maxLength; i++)
            {
                ulong a = (i < data.Length) ? data[i] : 0;
                ulong b = (i < other.data.Length) ? other.data[i] : 0;

                ulong sum = a + b + carry;
                result[i] = sum;
                carry = sum >> 64;
            }

            data = result;
        }

        public void Sub(BigInteger other)
        {
            if (IsLessThan(other))
            {
                throw new InvalidOperationException("Cannot subtract a larger number from a smaller number.");
            }

            ulong borrow = 0;

            for (int i = 0; i < data.Length; i++)
            {
                ulong a = data[i];
                ulong b = (i < other.data.Length) ? other.data[i] : 0;

                if (a < b + borrow)
                {
                    a += (1UL << 64) - b - borrow;
                    borrow = 1;
                }
                else
                {
                    a -= b + borrow;
                    borrow = 0;
                }

                data[i] = a;
            }

            TrimLeadingZeros();
        }

        public void Mul(BigInteger other)
        {
            int resultLength = data.Length + other.data.Length;
            ulong[] result = new ulong[resultLength];

            for (int i = 0; i < data.Length; i++)
            {
                ulong carry = 0;

                for (int j = 0; j < other.data.Length; j++)
                {
                    ulong product = data[i] * other.data[j] + result[i + j] + carry;
                    result[i + j] = product;
                    carry = product >> 64;
                }

                result[i + other.data.Length] = carry;
            }

            data = result;
            TrimLeadingZeros();
        }

        public void Div(BigInteger divisor, out BigInteger quotient, out BigInteger remainder)
        {
            if (divisor.IsZero())
            {
                throw new DivideByZeroException("Divisor cannot be zero.");
            }

            BigInteger dividend = new BigInteger(ToString());
            quotient = new BigInteger("0");
            remainder = new BigInteger("0");

            while (dividend.IsGreaterOrEqual(divisor))
            {
                BigInteger tempQuotient = new BigInteger("1");
                BigInteger tempResult = new BigInteger(ToString());
                BigInteger tempDivisor = new BigInteger(divisor.ToString());

                while (tempResult.IsGreaterOrEqual(tempDivisor))
                {
                    tempDivisor.ShiftLeft(1);
                    tempQuotient.ShiftLeft(1);
                }

                tempDivisor.ShiftRight(1);
                tempQuotient.ShiftRight(1);

                while (tempResult.IsGreaterOrEqual(divisor))
                {
                    if (tempResult.IsGreaterOrEqual(tempDivisor))
                    {
                        tempResult.Sub(tempDivisor);
                        tempQuotient.Add(BigInteger.One);
                    }
                    tempDivisor.ShiftRight(1);
                    tempQuotient.ShiftRight(1);
                }

                quotient.Add(tempQuotient);
                remainder = tempResult;
                dividend = tempResult;
            }
        }

        public void Mod(BigInteger modulus)
        {
            if (modulus.IsZero())
            {
                throw new DivideByZeroException("Modulus cannot be zero.");
            }

            while (IsGreaterOrEqual(modulus))
            {
                Sub(modulus);
            }
        }

        private void TrimLeadingZeros()
        {
            int leadingZeros = 0;

            for (int i = data.Length - 1; i > 0; i--)
            {
                if (data[i] == 0)
                {
                    leadingZeros++;
                }
                else
                {
                    break;
                }
            }

            if (leadingZeros > 0)
            {
                ulong[] newData = new ulong[data.Length - leadingZeros];
                Array.Copy(data, newData, newData.Length);
                data = newData;
            }
        }

        public bool IsZero()
        {
            return data.Length == 1 && data[0] == 0;
        }

        public bool IsGreaterThan(BigInteger other)
        {
            if (data.Length > other.data.Length)
            {
                return true;
            }
            if (data.Length < other.data.Length)
            {
                return false;
            }

            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (data[i] > other.data[i])
                {
                    return true;
                }
                if (data[i] < other.data[i])
                {
                    return false;
                }
            }

            return false;
        }

        public bool IsLessThan(BigInteger other)
        {
            if (data.Length < other.data.Length)
            {
                return true;
            }
            if (data.Length > other.data.Length)
            {
                return false;
            }

            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (data[i] < other.data[i])
                {
                    return true;
                }
                if (data[i] > other.data[i])
                {
                    return false;
                }
            }

            return false;
        }

        public bool IsOdd()
        {
            return (data[0] & 1) == 1;
        }
        
        public bool IsGreaterOrEqual(BigInteger other)
        {
            if (IsGreaterThan(other))
            {
                return true;
            }

            if (data.Length == other.data.Length)
            {
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    if (data[i] > other.data[i])
                    {
                        return true;
                    }
                    if (data[i] < other.data[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
}
