using System;

namespace zadacha4Avm
{
    class Program
    {
       static double ResidualCalculate(double x, double y, double[,]Slau)
        {
            return Math.Abs((Slau[0, 0] * x) + (Slau[0, 1] * y) - Slau[0, 2])
                 + Math.Abs((Slau[1, 0] * x) + (Slau[1, 1] * y) - Slau[1, 2]);
        }
        static double ZendelCalculation(double [,]Slau, double Epsilon1)
        {
            double Bn1 = Slau[0, 2];
            double Bn2 = Slau[1, 2];

            double a11 = Slau[0, 0];
            double a12 = Slau[0, 1];
            double a21 = Slau[1, 0];
            double a22 = Slau[1, 1];

            double OldX = 0;
            double OldY = 0;


            double NewX = (Bn1 - a12 * OldY) / a11;
            double NewY = (Bn2 - a21 * NewX) / a22;
            int Iterations = 1;
            Console.WriteLine("Норма невязки: " + ResidualCalculate(NewX, NewY, Slau));

            while (Math.Abs(NewX - OldX) >= Epsilon1 || Math.Abs(NewY - OldY) >= Epsilon1)
            {
                OldX = NewX;
                OldY = NewY;
                NewX = (Bn1 - a12 * OldY) / a11;
                NewY = (Bn2 - a21 * NewX) / a22;

                Console.WriteLine($"x = {NewX:F4}, y = {NewY:F4}");

                Console.WriteLine("Норма невязки: " + ResidualCalculate(NewX, NewY, Slau));
                Iterations++;
            }
            Console.WriteLine($"Точность достигнута: x = {NewX}, y = {NewY}");
            Console.WriteLine($"Количество итераций: = {Iterations} ");


            return NewX;
        }
        
        static double JacobiCalculation(double[,] Slau, double Epsilon)
        {
           
            double Bn1 = Slau[0, 2];
            double Bn2 = Slau[1, 2];

            double a11 = Slau[0, 0];
            double a12 = Slau[0, 1];
            double a21 = Slau[1, 0];
            double a22 = Slau[1, 1];
            
            double OldX = 0;
            double OldY = 0;

            
            double NewX = (Bn1 - a12 * OldY) / a11;
            double NewY = (Bn2 - a21 * OldX) / a22;
            int Iterations = 1;
            Console.WriteLine("Норма невязки: " + ResidualCalculate(NewX, NewY, Slau));

            while (Math.Abs(NewX - OldX) >= Epsilon || Math.Abs(NewY - OldY) >= Epsilon)
            {
                OldX = NewX;
                OldY = NewY;
                NewX = (Bn1 - a12 * OldY) / a11;
                NewY = (Bn2 - a21 * OldX) / a22;
                
                Console.WriteLine($"x = {NewX:F4}, y = {NewY:F4}");

                Console.WriteLine("Норма невязки: " + ResidualCalculate(NewX,NewY,Slau));
                Iterations++;
            }
            Console.WriteLine($"Точность достигнута: x = {NewX}, y = {NewY}");
            Console.WriteLine($"Количество итераций: = {Iterations} ");


            return NewX;
        }

        static double JacobiBuild(double[,] Slau)
        {
            double Epsilon = 0.0001;
            double result = JacobiCalculation(Slau,Epsilon);
            return result;
        }
      
        static double ZendelBuild(double[,]Slau)
        {
            double Epsilon1 = 0.0001;
            double  result1 = ZendelCalculation(Slau, Epsilon1);
            return result1;
        }

        static void Main()
        {
            double[,] Slau = {
                {10, 2, 3},
                {2, 3, 5}
            };

            Console.WriteLine("Выберите метод:");
            Console.WriteLine("1. Якоби");
            Console.WriteLine("2. Зейделя");
            int Choice = Convert.ToInt32(Console.ReadLine());

            switch (Choice)
            {
                case 1:
                    double result = JacobiBuild(Slau);
                    break;
                case 2:
                    double result1 = ZendelBuild(Slau);
                    break;
            }
        }
    }
}