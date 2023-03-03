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

    internal class InstrumentBE
    {
        static SerialPort serialPort;
        //private static SerialDataReceivedEventHandler dataReceived;

        static void Main(string[] args)
        {
            
            //Console.WriteLine(commandResponse);
            //commandResponse = SerialCommand("COM3", "readstatus");
            //Console.WriteLine(commandResponse);
            //commandResponse = SerialCommand("COM3", "readconf");
            //Console.WriteLine(commandResponse);
            //Console.ReadKey();

            
            //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
            string[] ComPorts = SerialPort.GetPortNames();
            //Console.Write(ComPorts);
            //Console.WriteLine("Hei");

            //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
            /*
            Console.WriteLine("The following COM ports exist:");
            foreach (string port in ComPorts)
            {
                Console.WriteLine(port);
            }
            Console.WriteLine("Enter port name:");
            string portName = Console.ReadLine();

            */






            //Console.ReadKey();
            /*
            serialPort = new SerialPort();
            serialPort.PortName = "COM3";
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.DataReceived += dataReceived;

            /*
            //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
            //Console.WriteLine("The following COM ports exist:");
            //foreach (string port in ComPorts)
            //{
            //    Console.WriteLine(port);
            //}
            //Console.WriteLine("Enter port name:");
            string portName = Console.ReadLine();

            Console.WriteLine("Enter baud rate (e.g. 9600):");
            int baudRate = Convert.ToInt32(Console.ReadLine());
            //SerialPort serialPort = new SerialPort(portName, baudRate);
            //serialPort.Open();
            Console.WriteLine("Enter message to send to Arduino:");
            string message = Console.ReadLine();
            //serialPort.WriteLine(message);
            Console.WriteLine("Message sent. Waiting for response...");
            //string response = serialPort.ReadLine();
            //Console.WriteLine("Response received: " + response);
            Console.ReadKey();
            //serialPort.Close();
            


            bool runInBackground = false;
            bool logToFile = false;

            // Check if the '-l' argument was provided
            if (args != null && args.Contains("-l"))
            {
                logToFile = true;
            }

            // Check if the '-b' argument was provided
            if (args != null && args.Contains("-b"))
            {
                runInBackground = true;
            }

            if (runInBackground)
            {
                // Run the program in the background
                Console.WriteLine("Running in the background...");
                if (logToFile)
                {
                    Console.WriteLine("Logging to file...");
                    // Perform some logging to file
                }
                // Perform some action here
                string result = "Succeeded running in background";
                Console.WriteLine(result);
            }
            else
            {
                // Run the program in the foreground
                Console.WriteLine("Running in the foreground...");
                if (logToFile)
                {
                    Console.WriteLine("Logging to file...");
                    // Perform some logging to file
                }
                // Perform some action here
                string result = "Succeeded running in foreground";
                Console.WriteLine(result);
            }


            */

            //TCP Socket Server
            string serverIP = "127.0.0.1";
            IPEndPoint endpoint = new IPEndPoint (IPAddress.Parse(serverIP), 5000);
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
                //Console.WriteLine(buffer);

                //if (buffer.Length >= 0)
                //{ 
                int bytesReceived = client.Receive(buffer); // feilmeldinger på denne, kommer ikke forbi, venter på buffer?

                //if (bytesReceived > 0)
                //{
                string commandReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                string commandResponse = SerialCommand("COM3", commandReceived);
                Console.WriteLine(commandResponse);
                //Console.WriteLine("Received command: " + commandReceived);
                //if (logToFile) WriteToLogFile("Received message" + commandReceived);

                //Send to client
                client.Send(Encoding.ASCII.GetBytes(commandResponse));
                client.Close();
                //}
                /*
                else
                {
                    Console.WriteLine("No connection");
                }
                    //if (logToFile) Console.WriteLine("Client disconnected...");
                //}
                */
            }
            
        }

        static string SerialCommand(string portName, string command)
        {
            int baudRate = 9600;
            string serialResponse = "";
            SerialPort serialPort = new SerialPort(portName, baudRate);

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

        /*
        private static void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string message = serialPort.ReadLine();
            //textBoxComReceived.AppendText(message);
            Console.WriteLine(message);
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

        private static string PassCommandToSerial(string commandReceived)
        {
            string[] commandsToSerial = { "readconf", "writeconf", "readscaled", "readstatus" };
            if (commandReceived.Substring(0, commandsToSerial[0].Length) == commandsToSerial[0])
            {
                //SerialSend "readconf" 
                //Read response
                //Return Response
                return "";
            }
            else if (commandReceived.Substring(0, commandsToSerial[1].Length) == commandsToSerial[1])
            {
                return "";
            }
            else if (commandReceived.Substring(0, commandsToSerial[2].Length) == commandsToSerial[2])
            {
                return "";
            }
            else if (commandReceived.Substring(0, commandsToSerial[3].Length) == commandsToSerial[3])
            {
                return "";
            }
            else //command unknown
            {
                return "";
            }
        }
        */
    }
}

//Endre navn til InstrumentBE
//Legg inn kode som spør Arduino om "readscaled", "readstatus" "writeconf" og "readconf"