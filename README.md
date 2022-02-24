# Async-Client-Server
This Repo contains two different Console Applications using the .NET framework (4.5 and up)

In the AsyncSocketServer folder, there is a subfolder named SocketAsync, in which the methods are implemented.

After Cloning the Repository, Open the AsyncSocketServer and AsyncSocketClient folders in two different windows in Visual Studio 2019 (If you want to connect multiple clients, open more windows of AsyncSocketClient.

Both AsyncSocketServer and AsyncSocketClient contain a Program.cs file. In order to run the server-client, you must first start the Program.cs located in the AsyncSocketServer folder, and then you are allowed to run as many of the AsyncSocketClient's Program.cs depending on how many connections you would like to make

Functionality: Currently, the Server tracks all connected clients and prints them to the console. As soon as a connection is madee, the Server sends the Screen Resolution, which displays on the Client Console.

While connected the Client is able to send text data by typing and pressing enter. To close a client connection, input "quit" and click enter on the Client terminal.
