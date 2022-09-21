
# Unity TCP-UDP Socket Example

TCP-UDP Socket Example for Unity

Server-Side operations were staged and programmed as a unity project.

![](https://raw.githubusercontent.com/burakpatat/UnityTCP-UDPSocketExample/main/img/1.png)

 
```bash 
    tcpListener = new TcpListener(IPAddress.Any, Port);
    tcpListener.Start();
    tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

    UdpListener = new UdpClient(Port);
    UdpListener.BeginReceive(UDPReceiveCallback, null);
```
    
a simple server log interface

![](https://raw.githubusercontent.com/burakpatat/UnityTCP-UDPSocketExample/main/img/2.png)
![](https://raw.githubusercontent.com/burakpatat/UnityTCP-UDPSocketExample/main/img/3.png)
![](https://raw.githubusercontent.com/burakpatat/UnityTCP-UDPSocketExample/main/img/4.png)


