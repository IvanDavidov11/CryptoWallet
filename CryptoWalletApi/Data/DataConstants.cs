namespace CryptoWalletApi.Data
{
    public static class DataConstants
    {
        //The total number of digits for prices and other precision numbers.
        //GPT says 18 is a good amount for anything related to crypto
        public const int DecimalPrecision_TotalDigits = 18;

        //The allowed amount of numbers after the decimal point, exam requirements specify 4. EX:N.XXXX
        public const int DecimalPrecision_DecimalPlaces = 4;

        public const int CoinNameLengthMaximum = 100;
    }
}
