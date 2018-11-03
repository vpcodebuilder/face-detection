using System;

namespace FaceDetection
{
    class Program
    {        
        static void Main(string[] args)
        {
            try
            {
                new Engine().Run(new FaceDetection());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
