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
            string fileNameSerialConfig = "serial.conf";
            string serialPortName = "";

            //Introdusksjon
            Console.WriteLine("instrumentBE has started...");
            
            Console.WriteLine("Please enter TCP Port number...");
            string serverPort = Console.ReadLine();

            try
            {
                int portNumber = Convert.ToInt32(serverPort);
            }
            catch(FormatException)
            {
                Console.WriteLine("Portnumber is not a number!");
                Console.WriteLine("Press a key to exit.");
                Console.ReadKey();
                return;
            }
            
            //Serial configuration Load from file
            //StreamWriter outputFile = new StreamWriter("serial.conf");
            StreamReader serialConfReader= new StreamReader(fileNameSerialConfig);
            serialPortName = serialConfReader.ReadLine();
            Console.WriteLine("Serial Port Configured: "+serialPortName);
            serialConfReader.Close();

            
            //Console.WriteLine(commandResponse);
            //commandResponse = SerialCommand("COM3", "readstatus");
            //Console.WriteLine(commandResponse);
            //commandResponse = SerialCommand("COM3", "readconf");
            //Console.WriteLine(commandResponse);
            //Console.ReadKey();
            
            
            string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
            //string[] ComPorts = SerialPort.GetPortNames();

            //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
            //client.Send(Encoding.ASCII.GetBytes(commandResponsePass));

            Console.WriteLine("The following COM ports exist:");
            foreach (string port in ComPorts)
            {
                //client.Send(port);
                Console.WriteLine(port);
            }
            
            //Console.WriteLine("Enter port name:");
            //string portName = Console.ReadLine();
            /*
            Console.WriteLine("Enter baud rate (e.g. 9600):");
            int baudRate = Convert.ToInt32(Console.ReadLine());
            SerialPort serialPort = new SerialPort(portName, baudRate);
            
            //client.Send(Encoding.ASCII.GetBytes("Serial PortName Configurated " + serialPortName));
            //client.Close();
            serialPort.Open();
            
            Console.WriteLine("Enter message to send to Arduino:");
            string message = Console.ReadLine();
            //serialPort.WriteLine(message);
            Console.WriteLine("Message sent. Waiting for response...");
            
            string response = serialPort.ReadLine();
            //Console.WriteLine("Response received: " + response);
            Console.ReadKey();
            serialPort.Close();
            
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

                //if (buffer.Length >= 0)
                //{ 
                int bytesReceived = client.Receive(buffer); // feilmeldinger på denne, kommer ikke forbi, venter på buffer?
                
                //if (bytesReceived > 0)
                //{
                string commandReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                //Console.WriteLine("HEEEEEEEEEEE"+commandReceived);
                //client.Send(commandReceived);
                //Console.WriteLine("Testong commandsssssssssss: " + commandReceived);

                if(commandReceived.Substring(0,7) == "comport")     //kommer bare ut "comport" og ikke "COM3"
                {
                    //Tester ut for å hente ut comports
                    //string[] COM = System.IO.Ports.SerialPort.GetPortNames();
                    //string[] ComPorts = SerialPort.GetPortNames();

                    string[] GettingPorts = System.IO.Ports.SerialPort.GetPortNames();      //feilmelding på denne
                    //client.Send(Encoding.ASCII.GetBytes(commandResponsePass));
                    
                    Console.WriteLine("The following COM ports exist:");
                    foreach (string Ports in GettingPorts)
                    {
                        client.Send(Encoding.ASCII.GetBytes("Portname configurated " + Ports));     //serialPortName er null, får ikke ut tilgjengelige porter
                        client.Close();
                        //Console.WriteLine("My ports"+Ports);
                    }
                    //string commandResponse = SerialCommand("COM3", commandReceived);
                    //Console.WriteLine("Command response was: " + commandResponse);


                    //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
                    //string[] ComPorts = SerialPort.GetPortNames();

                    //string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
                    //client.Send(Encoding.ASCII.GetBytes(commandResponsePass));


                    /*
                    //

                    string[] ComPorts = System.IO.Ports.SerialPort.GetPortNames();
                    Console.WriteLine("Comports testing: "+ComPorts);
                    
                    //Hente ut tilgjengelige comports
                    Console.WriteLine("The following COM ports exist:");
                    foreach (string port in ComPorts)
                    {
                        client.Send(Encoding.ASCII.GetBytes(port));
                        //Console.WriteLine(port);
                    }
                    */
                    /*
                    //
                    serialPortName= commandReceived.Substring(2, commandReceived.Length-2);   //denne var opprinnelig, endre på lengdene
                    //serialPortName = commandReceived.Substring(1, commandReceived.Length);      //noe galt med denne

                    //string commandResponse = SerialCommand(serialPortName, commandReceived);
                    //Console.WriteLine("Test" +commandResponse);
                    //Console.Write("Serial Port Configurated: " + serialPortName);
                    StreamWriter serialConfWrite = new StreamWriter(fileNameSerialConfig);
                    //serialConfWrite.WriteLine(serialPortName);
                    serialConfWrite.Close();
                    
                    
                    client.Send(Encoding.ASCII.GetBytes("Portname configurated "+serialPortName));     //serialPortName er null, får ikke ut tilgjengelige porter
                    client.Close();
                    */
                }

                else
                {
                    string commandResponse = SerialCommand(serialPortName, commandReceived);
                    Console.WriteLine("Command response was: "+commandResponse);


                    //Send to client
                    string commandResponsePass = PassCommandToSerial(commandReceived);
                    client.Send(Encoding.ASCII.GetBytes(commandResponsePass));
                    //client.Send(Encoding.ASCII.GetBytes(commandResponseParts));
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


                        //string commandResponse = SerialCommand(commandReceived);
                        //Console.WriteLine("Name: "+commandResponse[0]);

                        //string[] commandparts = commandReceived.Split(";");
                        //string[] commandToSerial = commandReceived.Split(";");
                        //Console.WriteLine(commandparts);

                        //Console.WriteLine("My response was: "+commandReceived+ "end");
                        //SerialSend "readconf" 
                        //Read response
                        //Return Response
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
                        //return "LRV: " + scaledparts[0] + "URV: " + scaledparts[1];
                        
                    }
                    else if (commandReceived.Substring(0, commandsToSerial[3].Length) == commandsToSerial[3])
                    {
                        //Readstatus
                        string serialResponseReadstatus = SerialCommand("COM3", commandReceived);

                        string[] scaledparts = serialResponseReadstatus.Split(delimiters);
                        /*
                        string[] levels = { "OK", "Fail", "Alarm Low", "Alarm High" };

                        string status{
                                        if (scaledparts[1] == "0")
                                        {
                                            return levels[0];
                                        }

                                        if (scaledparts[1] == "1")
                                        {
                                            return levels[1];
                                        }

                                        if (scaledparts[1] == "2")
                                        {
                                            return levels[2];
                                        }

                                        if (scaledparts[1] == "3")
                                        {
                                            return levels[3];
                                        }
                        }
                        
                        */
                        return "Status: " + scaledparts[1];         // finne ut hvordan jeg kan returnere med tekst "ok", "fail", "alamrL", "alarmH"
                    }
                    else //command unknown
                    {
                        return "Failed!";
                    }
                }

                
                //Console.WriteLine(PassCommandToSerial("readconf"));

                //Console.WriteLine("Received command: " + commandReceived);
                //if (logToFile) WriteToLogFile("Received message" + commandReceived);


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
            //SerialPort serialPort = new SerialPort(portName, baudRate);
            //
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
        */

       
        
    }
}

//Endre navn til InstrumentBE
//Legg inn kode som spør Arduino om "readscaled", "readstatus" "writeconf" og "readconf"