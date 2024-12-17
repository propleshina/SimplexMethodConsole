using System;

class Simplex
{
    public static void Main(string[] args)
    {
        Console.Write("Введите количество переменных: ");
        int numVars = int.Parse(Console.ReadLine());

        Console.Write("Введите количество ограничений: ");
        int numConstraints = int.Parse(Console.ReadLine());

        double[,] tableau = new double[numConstraints + 1, numVars + numConstraints + 1];

        Console.WriteLine("Введите коэффициенты целевой функции:");
        string[] objectiveCoefficients = Console.ReadLine().Split(' ');
        for (int j = 0; j < numVars; j++)
        {
            tableau[numConstraints, j] = double.Parse(objectiveCoefficients[j]);
        }

        for (int i = 0; i < numConstraints; i++)
        {
            Console.WriteLine($"Ограничение {i + 1} (в формате: x1 x2 ... xn b):");
            string[] constraintInput = Console.ReadLine().Split(' ');
            for (int j = 0; j < numVars; j++)
            {
                tableau[i, j] = double.Parse(constraintInput[j]);
            }
            tableau[i, numVars + i] = 1; 
            tableau[i, numVars + numConstraints] = double.Parse(constraintInput[numVars]);
        }

        for (int j = 0; j < numVars; j++)
        {
            tableau[numConstraints, j] *= -1; 
        }

        SimplexMethod(tableau);
    }

    public static void SimplexMethod(double[,] tableau)
    {
        int numRows = tableau.GetLength(0);
        int numCols = tableau.GetLength(1);

        while (true)
        {
            int pivotCol = -1;
            for (int j = 0; j < numCols - 1; j++)
            {
                if (tableau[numRows - 1, j] < 0)
                {
                    pivotCol = j;
                    break;
                }
            }

            if (pivotCol == -1)
                break;

            int pivotRow = -1;
            double minRatio = double.MaxValue;

            for (int i = 0; i < numRows - 1; i++)
            {
                if (tableau[i, pivotCol] > 0)
                {
                    double ratio = tableau[i, numCols - 1] / tableau[i, pivotCol];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            if (pivotRow == -1)
            {
                Console.WriteLine("Неограниченное решение.");
                return;
            }

            double pivotValue = tableau[pivotRow, pivotCol];
            for (int j = 0; j < numCols; j++)
                tableau[pivotRow, j] /= pivotValue;

            for (int i = 0; i < numRows; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotCol];
                    for (int j = 0; j < numCols; j++)
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                }
            }
        }

        Console.WriteLine("Оптимальное решение:");
        for (int i = 0; i < numRows - 1; i++)
        {
            Console.WriteLine($"x{i + 1} = {tableau[i, numCols - 1]}");
        }
        Console.WriteLine($"Максимальное значение F = {-tableau[numRows - 1, numCols - 1]}");
 
        Console.WriteLine("\nФинальная таблица симплекс-метода:");
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Console.Write($"{tableau[i, j]:F2}\t");
            }
            Console.WriteLine();
        }
        Console.ReadLine();
    }
}
