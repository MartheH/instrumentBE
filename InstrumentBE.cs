namespace ConsoleAppProjekt
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Transactions;
    using System.Threading;
    using System.Diagnostics.Tracing;
    using System.IO.Ports;
    using System.ComponentModel.Design;
    using System.Xml.Linq;

    internal class InstrumentBE
    {
        static SerialPort serialPort;


        static void Main(string[] args)
        {
            //Introdusksjon
            Console.WriteLine("instrumentBE has started...");

            //TCP Socket Server
            string serverIP = "127.0.0.1";
            //string serverIP = client.Port;
            int TCPReceived = 5000;
            IPEndPoint endpoint = new IPEndPoint (IPAddress.Parse(serverIP), TCPReceived);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //bind to endpoint and start server
            try
            {
                server.Bind(endpoint);
                server.Listen(10);
            }

            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exiting...");
                Console.ReadKey();
                return;
            }

            //output info
            Console.WriteLine("Server started. Waiting for clients...");
            /*
            if (logToFile)
            {
                WriteToLogFile("Server started. Waiting for connection...");
            }
            */

            while (true)
            {

                Socket client = server.Accept();
                Console.WriteLine("Client connected.");
                //if (logToFile) WriteToLogFile("Client connected.");

                /*
                if (false)
                {
                    Console.WriteLine("Client is not connected");
                    break;
                }
                */

                //data received
                byte[] buffer = new byte[1024]; 

                int bytesReceived = client.Receive(buffer);
                //WriteToLogFile(Convert.ToString(buffer));
                string commandReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                //WriteToLogFile(Convert.ToString(commandReceived));
                if (commandReceived.Substring(0,7) == "comport")
                {
                    string[] GettingPorts = System.IO.Ports.SerialPort.GetPortNames();

                    foreach (string Ports in GettingPorts)
                    {
                        client.Send(Encoding.ASCII.GetBytes("Portname configurated " + Ports));
                        client.Close();
                    }
                }

                else
                {
                    //Send to client
                    string commandResponsePass = PassCommandToSerial(commandReceived);
                    client.Send(Encoding.ASCII.GetBytes(commandResponsePass));
                    client.Close();
                    Console.WriteLine("Client disconnected...");   
                }
 
                static string PassCommandToSerial(string commandReceived)
                {
                    char[] delimiters = { '.', ';' };
                    string[] commandsToSerial = { "readconf", "writeconf", "readscaled", "readstatus" };
                    if (commandReceived.Substring(0, commandsToSerial[0].Length) == commandsToSerial[0])
                    {
                        string serialResponseConf = SerialCommand("COM3", commandReceived);
                        string[] confparts = serialResponseConf.Split(";");
                        return "Name: "+ confparts[1] + "LRV: "+ confparts[2] + "URV: "+ confparts[3]+ "Alarm Low: "+ confparts[4] + "Alarm High: "+ confparts[5];
                    }
                    else if (commandReceived.Substring(0, commandsToSerial[1].Length) == commandsToSerial[1])
                    {
                        //Writeconf
                        string serialResponseWriteConf = SerialCommand("COM3", commandReceived);
                        string[] writeConfparts = serialResponseWriteConf.Split(delimiters);

                        return "Write Configuration status: " + writeConfparts[1];
                    }
                    else if (commandReceived.Substring(0, commandsToSerial[2].Length) == commandsToSerial[2])
                    {
                        //Readscaled
                        string serialResponseScaled = SerialCommand("COM3", commandReceived);

                        string[] scaledparts = serialResponseScaled.Split(';');
                        return scaledparts[1]; //sender ikke ut samme verdier som blir skrevet ut i console-vindu
                    }
                    else if (commandReceived.Substring(0, commandsToSerial[3].Length) == commandsToSerial[3])
                    {
                        //Readstatus
                        string serialResponseReadstatus = SerialCommand("COM3", commandReceived);

                        string[] scaledparts = serialResponseReadstatus.Split(delimiters);
                        return "Status: " + scaledparts[1];
                    }
                    
                    else
                    {
                        return "Failed!";
                    }
                }
            }  
        }

        static string SerialCommand(string portName, string command)
        {
            int baudRate = 9600;
            string serialResponse = "";
            SerialPort serialPort = new SerialPort("COM3", baudRate);

            try
            {
                serialPort.Open();
                serialPort.Write(command);
                serialResponse = serialPort.ReadLine();
                serialPort.Close();
            }

            catch (System.IO.IOException) 
            {
                serialResponse = "Command failed...";
            }
            return serialResponse;
        }
        
        private static void WriteToLogFile(string logText)
        {
            string fileName = "log.txt";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            // Open the file for writing
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read)) 
            {
                using (StreamWriter sw = new StreamWriter(fs)) 
                {
                        sw.WriteLine("" + System.DateTime.Now + "" + logText + "\r\n");
                }    
                fs.Close();
            }
        }
    }
}

//Endre navn til InstrumentBE
//Legg inn kode som spør Arduino om "readscaled", "readstatus" "writeconf" og "readconf"