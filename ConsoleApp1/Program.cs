namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string number = "202?";
            char[] digit = new char[number.Length];
            int sum = 0;
            int sum2 = 0;
            int index = 0;
            int result = 0;
            for(int i=0;i<number.Length; i++)
            {
                digit[i] =number[i];

            }
            for(int i=0;i<digit.Length; i++)
            {
                if (digit[i] == '?') {
                    index = i + 1;
                    continue;
                }
                else
                {
                    sum += int.Parse(digit[i].ToString())*(i+1);
                }
            }
            for(int i=0;i<9;i++)
            {
                sum2 = sum + (i * index);
                if (sum2 % 7 == 0)
                {
                    result = i;
                    break;
                }
            }
            
            Console.WriteLine(result);
        }
    }
}