
namespace BetKanu.Models.Helper
{
    public static class EncryptionHelper
    {
        private static readonly string EncryptionKey = "Betkanusecret";

        public static string Encrypt(int id)
        {
            byte[] plainBytes = BitConverter.GetBytes(id);
            string encryptedString = Convert.ToBase64String(plainBytes);
            return encryptedString;
        }

        public static int Decrypt(string encryptedId)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedId);
            int id = BitConverter.ToInt32(encryptedBytes, 0);
            return id;
        }

    }
}
