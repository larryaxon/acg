using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  public class EncryptDecryptString
  {
    //private const string alphabet = "OPQRSTUVWXYZABCDEFGHIJKLMN1234567890@#$!opqrstuvwxyzabcdefghijklmn";
    private const string alphabet = "POIUYWQERTLKJHGFDSAMNBVCXZrewqtyuipofdsgahkjlmbnvxcz3975846012@#$!-";
    public const string BADPASSWORD = "BADPASSWORD";

    public string encryptString(string inputValue)
    {
      bool badPassword = false;

      if (inputValue == "")
        return "";

      if (inputValue.IndexOf(" ") >= 0)
        badPassword = true;
      else
      {
        for (int i = 0; i < inputValue.Length; i++)
        {
          if (!(alphabet.Contains(inputValue.Substring(i, 1))))
            badPassword = true;
          break;
        }
      }

      if (badPassword)
        throw new Exception(BADPASSWORD);

      string encryptedValue1 = "";
      string encryptedValue2 = "";
      for (int i = 0; i < inputValue.Length; i++) //Shuffle this array!
      {
        if (i == 0)
          encryptedValue1 = inputValue.Substring(inputValue.Length - 1, 1);
        else if (i == inputValue.Length - 1)
          encryptedValue1 = encryptedValue1 + inputValue.Substring(0, 1);
        else if (i % 2 != 0)
        {
          if (i + 1 < inputValue.Length - 1)
          {
            encryptedValue1 += inputValue.Substring(i + 1, 1) + inputValue.Substring(i, 1);
          }
          else
            encryptedValue1 += inputValue.Substring(i, 1);
        }
      }

      int alphabetLength = alphabet.Length;
      for (int i = 0; i < encryptedValue1.Length; i++)
      {
        int j = alphabet.IndexOf(encryptedValue1.Substring(i, 1));
        encryptedValue2 += alphabet.Substring(((i + j + 1) % alphabetLength), 1);
      }

      return encryptedValue2;
    }

    public string decryptString(string encryptedValue)
    {
      if (encryptedValue == "")
        return "";

      string decryptedValue1 = "";
      string decryptedValue2 = "";

      int alphabetLength = alphabet.Length;
      for (int i = 0; i < encryptedValue.Length; i++)
      {
        int j = alphabet.IndexOf(encryptedValue.Substring(i, 1));
        int alphabetTokenIndex = 0;
        if ((j - i - 1) < 0)
        {
          if ((alphabetLength + (j - i - 1)) >= 0)
            alphabetTokenIndex = (alphabetLength + (j - i - 1));
          else
            alphabetTokenIndex = ((1 + i - j) % alphabetLength);
        }
        else
          alphabetTokenIndex = ((j - i - 1) % alphabetLength);

        decryptedValue1 += alphabet.Substring(alphabetTokenIndex, 1); //Must use % too! because pwd could be greater the alphabet length!!
        //decryptedValue1 += alphabet.Substring(((j - i - 1) < 0) ? alphabetLength + (j - i - 1) : (j - i - 1), 1); //Must use % too! because pwd could be greater the alphabet length!!
      }

      for (int i = 0; i < decryptedValue1.Length; i++) //Shuffle this array!
      {
        if (i == 0)
          decryptedValue2 = decryptedValue1.Substring(decryptedValue1.Length - 1, 1);
        else if (i == decryptedValue1.Length - 1)
          decryptedValue2 = decryptedValue2 + decryptedValue1.Substring(0, 1);
        else if (i % 2 != 0)
        {
          if (i + 1 < decryptedValue1.Length - 1)
          {
            decryptedValue2 += decryptedValue1.Substring(i + 1, 1) + decryptedValue1.Substring(i, 1);
          }
          else
            decryptedValue2 += decryptedValue1.Substring(i, 1);
        }
      }

      return decryptedValue2;
    }
  }

}
