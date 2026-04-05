using UnityEngine;

public class A4A1Z : MonoBehaviour
{
    public int nameLength = 10;

    void Update()
    {     
        gameObject.name = HideName(nameLength);
    }

    private string HideName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#%&/()";
        char[] stringChars = new char[length];
        System.Random random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}
