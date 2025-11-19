using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("решение слау\n");
        double[,] mat = {
            {4.8, 1, 1},
            {1, 3.1, 1},
            {1, 1, 5.3}
        };
        double[] right = { 1, 2, 3.2 };
        Console.WriteLine("матрица а:");
        ShowMat(mat);
        Console.WriteLine("\nвектор f: [" + string.Join(", ", right) + "]");
        bool symmetric = CheckSymmetric(mat);
        Console.WriteLine($"\nматрица симметричная: {(symmetric ? "да" : "нет")}");
        //гаусс
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("гаусс");
        Console.WriteLine(new string('=', 40));
        double[] res1 = Gauss(mat, right);
        Console.WriteLine("решение: " + FormatRes(res1));
        ShowDiff(mat, res1, right);

        //холецкий
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("холецкий");
        Console.WriteLine(new string('=', 40));

        if (symmetric)
        {
            double[] res2 = Hol(mat, right);
            Console.WriteLine("решение: " + FormatRes(res2));
            ShowDiff(mat, res2, right);

            //проверка
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("проверка");
            Console.WriteLine(new string('=', 40));
            CheckRes(mat, right, res1, res2);
        }
        else
        {
            Console.WriteLine("не симметричная");
        }
    }
    //симметрия
    static bool CheckSymmetric(double[,] a)
    {
        int n = a.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(a[i, j] - a[j, i]) > 1e-10)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //krasata
    static string FormatRes(double[] x)
    {
        return "[" + string.Join(", ", x.Select(val =>
            Math.Abs(val) < 1e-10 ? "0" : val.ToString("F6"))) + "]";
    }

    //гаусс
    static double[] Gauss(double[,] a, double[] b)
    {
        int n = b.Length;
        double[,] m = new double[n, n + 1];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                m[i, j] = a[i, j];
            m[i, n] = b[i];
        }
        for (int i = 0; i < n; i++)
        {
            double d = m[i, i];
            for (int j = i; j <= n; j++)
                m[i, j] /= d;

            for (int k = i + 1; k < n; k++)
            {
                double f = m[k, i];
                for (int j = i; j <= n; j++)
                    m[k, j] -= f * m[i, j];
            }
        }
        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            x[i] = m[i, n];
            for (int j = i + 1; j < n; j++)
                x[i] -= m[i, j] * x[j];
        }

        return x;
    }

    //холецкий
    static double[] Hol(double[,] a, double[] b)
    {
        int n = b.Length;
        double[,] l = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                double s = 0;
                for (int k = 0; k < j; k++)
                    s += l[i, k] * l[j, k];

                if (i == j)
                    l[i, j] = Math.Sqrt(a[i, i] - s);
                else
                    l[i, j] = (a[i, j] - s) / l[j, j];
            }
        }

        double[] y = new double[n];
        for (int i = 0; i < n; i++)
        {
            double s = 0;
            for (int j = 0; j < i; j++)
                s += l[i, j] * y[j];
            y[i] = (b[i] - s) / l[i, i];
        }

        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            double s = 0;
            for (int j = i + 1; j < n; j++)
                s += l[j, i] * x[j];
            x[i] = (y[i] - s) / l[i, i];
        }

        return x;
    }

    //невязка
    static void ShowDiff(double[,] a, double[] x, double[] b)
    {
        int n = b.Length;
        double[] r = new double[n];

        for (int i = 0; i < n; i++)
        {
            double s = 0;
            for (int j = 0; j < n; j++)
                s += a[i, j] * x[j];
            r[i] = s - b[i];
        }

        Console.WriteLine("невязка: " + FormatRes(r));
        double norm = Math.Sqrt(r.Sum(val => val * val));
        Console.WriteLine("норма: " + (norm < 1e-10 ? "~0" : norm.ToString("E2")));
    }

    //проверка
    static void CheckRes(double[,] a, double[] b, double[] x1, double[] x2)
    {
        int n = b.Length;

        Console.WriteLine("\nпроверка решений:\n");

        for (int i = 0; i < n; i++)
        {
            double s1 = 0;
            double s2 = 0;
            for (int j = 0; j < n; j++)
            {
                s1 += a[i, j] * x1[j];
                s2 += a[i, j] * x2[j];
            }

            Console.WriteLine($"уравнение {i + 1}:");
            Console.WriteLine($"  гаусс:  {s1:F6} = {b[i]}");
            Console.WriteLine($"  холецкий: {s2:F6} = {b[i]}");
        }

        bool same = true;
        for (int i = 0; i < n; i++)
        {
            if (Math.Abs(x1[i] - x2[i]) > 1e-10)
            {
                same = false;
                break;
            }
        }

        Console.WriteLine($"\nсовпадение: {(same ? "да" : "нет")}");
    }
    //вывод матрицы
    static void ShowMat(double[,] m)
    {
        int r = m.GetLength(0);
        int c = m.GetLength(1);
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
                Console.Write(m[i, j].ToString("F1") + "\t");
            Console.WriteLine();
        }
    }
}