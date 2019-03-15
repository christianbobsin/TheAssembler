using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(args[0], Encoding.UTF8);            

            StreamWriter fileWriter = new StreamWriter( ChangeExtension( args[0] ) );

            foreach (var line in lines)
            {
                if (line.Trim().Length > 0)
                {
                    uint convertedValue = AssemblyLine(line);
                    fileWriter.WriteLine(convertedValue.ToString("D10"));
                }
            }

            fileWriter.Close();
        }

        private static string ChangeExtension(string v)
        {

            return $"{ v.Split('.')[0] }.dotti";
        }

        private static uint AssemblyLine(string line)
        {
            string lineClear = Regex.Replace(line, " {2,}", " ");
            var command = lineClear.Trim().Split(',');

            uint value = ConvertMnemonic(command[0]);

            // Value = 0 significa que a instrucao
            if (value == 0)
            {
                Console.WriteLine($"Comando Inválido: { line }");
                Console.WriteLine("Montagem Interrompida!");

                Environment.Exit(0);
            }

            if (command[0].ToUpper() != "STOP")
            {
                if (command[1].Length > 0)
                {
                    if (uint.TryParse(command[1], out uint immediate))
                    {
                        value |= immediate;
                    }
                    else
                    {
                        value |= ConvertRegisterValue(command[1]);
                    }
                }
            }
            return value;
        }

        private static uint ConvertMnemonic(string v)
        {
            var instruction = v.Split(' ');

            uint value = 0;

            switch (instruction[0].ToUpper())
            {
                case "JMP":
                    value = 0b0000_0000_0000_0000_0000_0000_0000_0000;
                    break;

                case "JMPI":
                    value = 0b0000_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "JMPIG":
                    value = 0b0001_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "JMPIL":
                    value = 0b0001_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "JMPIE":
                    value = 0b0010_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "ADDI":
                    value = 0b0100_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "SUBI":
                    value = 0b0100_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "ANDI":
                    value = 0b0101_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;


                case "ORI":
                    value = 0b0101_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "LDI":
                    value = 0b0110_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "LDD":
                    value = 0b0110_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "STD":
                    value = 0b0111_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "ADD":
                    value = 0b1000_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "SUB":
                    value = 0b1000_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "AND":
                    value = 0b1001_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "OR":
                    value = 0b1001_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "LDX":
                    value = 0b1010_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "STX":
                    value = 0b1010_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "NOT":
                    value = 0b1100_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "SHL":
                    value = 0b1100_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "SHR":
                    value = 0b1101_0000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "SWAP":
                    value = 0b1101_1000_0000_0000_0000_0000_0000_0000;
                    value |= ConvertRegisterValue(instruction[1]);
                    break;

                case "STOP":
                    value = 0b1111_1000_0000_0000_0000_0000_0000_0000;
                    break;

                default:
                    break;
            }
            return value;
        }

        private static uint ConvertRegisterValue(string v)
        {
            uint value = 0;
            switch (v.ToUpper())
            {
                case "R0":
                    value = 0b0000_0000_0000_0000_0000_0000_0000_0000;
                    break;
                case "R1":
                    value = 0b0000_0001_0000_0000_0000_0000_0000_0000;
                    break;
                case "R2":
                    value = 0b0000_0010_0000_0000_0000_0000_0000_0000;
                    break;
                case "R3":
                    value = 0b0000_0011_0000_0000_0000_0000_0000_0000;
                    break;
                case "R4":
                    value = 0b0000_0100_0000_0000_0000_0000_0000_0000;
                    break;
                case "R5":
                    value = 0b0000_0101_0000_0000_0000_0000_0000_0000;
                    break;
                case "R6":
                    value = 0b0000_0110_0000_0000_0000_0000_0000_0000;
                    break;
                case "R7":
                    value = 0b0000_0111_0000_0000_0000_0000_0000_0000;
                    break;

            }
            return value;
        }
    }
}
