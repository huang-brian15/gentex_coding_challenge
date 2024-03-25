using System;
using System.IO;

namespace CodingChallenge
{
    public abstract class Shape
    {
        public abstract double calculateArea();
        public abstract double calculatePerimeter();
    }
    

    public class Ellipse: Shape
    {
        double centerX, centerY, r1, r2, orientation, area, perimeter;

        public Ellipse(double centerX, double centerY, double r1, double r2, double orientation)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.r1 = r1;
            this.r2 = r2;
            this.orientation = orientation;
        }

        public override double calculateArea()
        {
            area = Math.PI * r1 * r2;
            return area;
        }        
        
        public override double calculatePerimeter()
        {
            perimeter = Math.PI * (3.0*(r1 + r2) - Math.Sqrt((3.0*r1 + r2)*(r1 + 3.0*r2)));
            return perimeter;
        }
    }
    

    public class Circle: Shape
    {
        double centerX, centerY, r1, area, perimeter;

        public Circle(double centerX, double centerY, double r1)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.r1 = r1;
        }

        public override double calculateArea()
        {
            area = Math.PI * Math.Pow(r1, 2.0);
            return area;
        }

        public override double calculatePerimeter()
        {  
           perimeter = 2.0 * Math.PI * r1;
           return perimeter;
        }
    }


    public class Equilateral_Triangle: Shape
    {
        double centerX, centerY, side_len, orientation, area, perimeter;

        public Equilateral_Triangle(double centerX, double centerY, double side_len, double orientation)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.side_len = side_len;
            this.orientation = orientation;
        }

        public override double calculateArea()
        {
            area = (Math.Sqrt(3.0) / 4.0) * Math.Pow(side_len, 2.0);
            return area;
        }

        public override double calculatePerimeter()
        {
            perimeter = 3.0 * side_len;
            return perimeter;
        }
    }


    public class Square: Shape
    {
        double centerX, centerY, side_len, orientation, area, perimeter;

        public Square(double centerX, double centerY, double side_len, double orientation)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.side_len = side_len;
            this.orientation = orientation;
        }

        public override double calculateArea()
        {
            area = Math.Pow(side_len, 2.0);
            return area;
        }

        public override double calculatePerimeter()
        {
            perimeter = side_len * 4.0;
            return perimeter;
        }
    }


    public class Polygon: Shape
    {
        double[,] coordinates;
        double area = 0;
        double perimeter = 0;

        public Polygon(double[,] coordinates)
        {
            this.coordinates = coordinates;
        }

        public override double calculateArea()
        {   
            // Using https://en.wikipedia.org/wiki/Shoelace_formula
            for (int i = 0; i < coordinates.GetLength(0) - 1; i++)
            {
                area += coordinates[i, 0] * coordinates[i+1, 1];
                area -= coordinates[i+1, 0] * coordinates[i, 1];
            }
            area /= 2.0;
            return area;
        }

        public override double calculatePerimeter()
        {
            for (int i = 0; i < coordinates.GetLength(0) - 1; i++)
            {
                double euclidean_dist = Math.Sqrt(Math.Pow(coordinates[i+1, 1] - coordinates[i, 1], 2) + Math.Pow(coordinates[i+1, 0] - coordinates[i, 0], 2));
                perimeter += euclidean_dist;
            }
            return perimeter;
        }
    }


    public class UnknownShape: Shape
    {
        public override double calculateArea()
        {
            return -1.0;
        }

        public override double calculatePerimeter()
        {
            return -1.0;
        }
    }


    class Program
    {   
        /*
        This method takes in an array of strings with each string representing
        a value which was previously comma-separated.

        It parses the values and returns an instance of an object correlating
        to the correct shape.
        */
        static Shape getShapeInstance(string[] data)
        {
            string shape_type = data[1];
            Shape shape_obj;

            /* 
            Assumes all data in CSV file to be formatted perfectly for each shape.
            If not formatted perfectly, we need to handle parsing differently.

            Also does not account for missing or incorrect values. If values are 
            missing or incorrect, error handling should be implemented.
            */

            if (shape_type == "Ellipse")
            {
                double centerX = double.Parse(data[3]);
                double centerY = double.Parse(data[5]);
                double r1 = double.Parse(data[7]);
                double r2 = double.Parse(data[9]);
                double orientation = double.Parse(data[11]);
                shape_obj = new Ellipse(centerX, centerY, r1, r2, orientation);
            }
            else if (shape_type == "Circle")
            {
                double centerX = double.Parse(data[3]);
                double centerY = double.Parse(data[5]);
                double r1 = double.Parse(data[7]);
                shape_obj = new Circle(centerX, centerY, r1);
            }
            else if (shape_type == "Equilateral Triangle")
            {
                double centerX = double.Parse(data[3]);
                double centerY = double.Parse(data[5]);
                double side_len = double.Parse(data[7]);
                double orientation = double.Parse(data[9]);
                shape_obj = new Equilateral_Triangle(centerX, centerY, side_len, orientation);
            }
            else if (shape_type == "Square")
            {
                double centerX = double.Parse(data[3]);
                double centerY = double.Parse(data[5]);
                double side_len = double.Parse(data[7]);
                double orientation = double.Parse(data[9]);
                shape_obj = new Square(centerX, centerY, side_len, orientation);
            }
            else if (shape_type == "Polygon")
            {
                double[,] coordinates = new double[(data.Length - 2) / 4, 2];
                int idx = 0;
                for (int i = 3; i < data.Length; i += 4)
                {
                    coordinates[idx, 0] = double.Parse(data[i]);
                    coordinates[idx, 1] = double.Parse(data[i+2]);
                    idx += 1;
                }
                shape_obj = new Polygon(coordinates);
            }
            else
            {
                shape_obj = new UnknownShape();
            }

            return shape_obj;
        }

        static void Main(string[] args)
        {
            // Must specify input and output file paths
            if (args.Length != 3)
            {
                Console.WriteLine("Specify input file and output file.");
                return;
            }

            // Must be valid file paths
            string inputPath = args[1];
            string outputPath = args[2];

            // Check input path exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File path {0} does not exist. Exiting..", inputPath);
                return;
            }
            
            // Check if overwriting output path
            if (File.Exists(outputPath))
            {
                Console.WriteLine("File path {0} exists already. It will be overwritten. Continue? (Y/N)", outputPath);
                string overwrite = Console.ReadLine().ToUpper();
                if (overwrite == "N")
                {
                    Console.WriteLine("Exiting..");
                    return;
                }
            }

            // Read CSV file and output to new CSV file
            using (StreamReader reader = new StreamReader(inputPath))
            {
                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        
                        // Assuming shape ID to be integer; otherwise, implement error handling
                        long shapeID = Int64.Parse(values[0]);
                        string shape_type = values[1];
                        
                        Shape shape_obj = getShapeInstance(values);
                        if (shape_obj is UnknownShape)
                        {
                            Console.WriteLine("Unknown shape found with shape ID {0}. Skipping..", shapeID);
                            continue;
                        }

                        double area = shape_obj.calculateArea();
                        double perimeter = shape_obj.calculatePerimeter();
                        
                        // Create new line to output to CSV file
                        string newline = shapeID + "," + shape_type + ",Area," + area.ToString() + ",Perimeter," + perimeter.ToString();
                        writer.WriteLine(newline);
                    }
                }
            }
        }
    }
}